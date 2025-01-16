using Agency.Utilities.Enums;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace Agency.Utilities.Extensions
{
    public static class FileValidator
    {
        public static bool ValidateType(this IFormFile file, string type)
        {
            if (file.ContentType.Contains(type))
            {
                return true;
            }
            return false;
        }

        public static bool ValidateSize(this IFormFile file,FileSize filesize,int size)
        {
            switch (filesize)
            {
                case FileSize.KB:
                    return file.Length<=size*1024;
                case FileSize.MB:
                    return file.Length<=size*1024*1024;
                case FileSize.GB:
                    return file.Length<=size*1024*1024*1024;
                default:
                    break;
            }
            return false;
        }

        public async static Task<string> CreateFileAsync(this IFormFile file,params string[] roots)
        {
            string filename=string.Concat(Guid.NewGuid().ToString(),file.FileName);
            string path=string.Empty;
            
            for(int i = 0; i < roots.Length; i++)
            {
             path=Path.Combine(path,roots[i]);   
            }
           path=Path.Combine(path,file.FileName);

            using(FileStream fileStream=new (path,FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return filename;
        }

        public static void DeleteFile(this string filename,params string[] roots)
        {
           string path=string.Empty ;
            for(int i = 0;i < roots.Length; i++)
            {
                path=Path.Combine(path,roots[i]);
            }

            path=Path.Combine (path,filename);
            if(File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
