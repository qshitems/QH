
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FileInfo = QH.Core.Files.FileInfo;
using QH.Core.Configs;
using QH.Core.Extensions;
using QH.Core.Output;
using QH.Core.Attributes;
using QH.Core.Result;

namespace QH.Core.Helpers
{
    /// <summary>
    /// 文件上传帮助类
    /// </summary>
    [SingleInstance]
    public class UploadHelper
    {
        /// <summary>
        /// 上传单文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="config"></param>
        /// <param name="args"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IResultModel> UploadAsync(IFormFile file, FileUploadConfig config, object args, CancellationToken cancellationToken = default)
        {
           // var res = new ResultModel<FileInfo>();

            if (file == null || file.Length < 1)
            {
                return ResultModel.Failed("请上传文件！");
            }

            //格式限制
            if (!config.ContentType.Contains(file.ContentType))
            {
                return ResultModel.Failed("文件格式错误");
            }

            //大小限制
            if (!(file.Length <= config.MaxSize))
            {
                return ResultModel.Failed("文件过大");
            }

            var fileInfo = new FileInfo(file.FileName, file.Length)
            {
                UploadPath = config.UploadPath,
                RequestPath = config.RequestPath
            };

            var dateTimeFormat = config.DateTimeFormat.NotNull() ? DateTime.Now.ToString(config.DateTimeFormat) : "";
            var format = config.Format.NotNull() ? StringHelper.Format(config.Format, args) : "";
            fileInfo.RelativePath = Path.Combine(dateTimeFormat, format).ToPath();

            if (!Directory.Exists(fileInfo.FileDirectory))
            {
                Directory.CreateDirectory(fileInfo.FileDirectory);
            }

            fileInfo.SaveName = $"{IdWorkerHelper.GenId64()}.{fileInfo.Extension}";

            await SaveAsync(file, fileInfo.FilePath, cancellationToken);

            return ResultModel.Success(fileInfo);
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="filePath"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task SaveAsync(IFormFile file, string filePath, CancellationToken cancellationToken = default)
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream, cancellationToken);
            }
        }
    }
}
