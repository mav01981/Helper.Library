
using Helper.FileConvertor.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace Helper.FileConvertor
{
    public class ByteConvertor : IByteConvertor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public byte[] ConvertToBytes(string path)
        {
            return File.ReadAllBytes(path);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileEnum"></param>
        /// <param name="bytes"></param>
        public void ConvertBytesToFile(string filePath, FileExtensionsEnums fileEnum, byte[] bytes)
        {
            var fileExtension = GetDescription(fileEnum);

            File.WriteAllBytes($"{filePath}{fileExtension}", bytes);
        }

        public static  byte[] ObjectToByteArray<T>(T obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }

        private static string GetDescription(Enum @enum)
        {
            Type genericEnumType = @enum.GetType();
            MemberInfo[] memberInfo = genericEnumType.GetMember(@enum.ToString());
            if ((memberInfo.Length > 0))
            {
                var attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if ((attribs.Any()))
                {
                    return ((System.ComponentModel.DescriptionAttribute)attribs.ElementAt(0)).Description;
                }
            }
            return @enum.ToString();
        }
    }
}
