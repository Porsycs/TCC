using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SiteJogos.Core.Entities
{
    public static class Common
    {
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText ?? string.Empty);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData ?? string.Empty);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string RemoveCaratereEspecial(string? texto)
        {
            if (texto == null) return string.Empty;
            var regex = new Regex("[^a-zA-Z0-9]");
            return regex.Replace(RemoveAcentos(texto), "");
        }

        public static string RemoveAcentos(string? texto) 
        {
            if(texto == null) return string.Empty;
            string comAcentos = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
            string semAcentos = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc";
            for (int i = 0; i < comAcentos.Length; i++)
            {
                texto = texto.Replace(comAcentos[i].ToString(), semAcentos[i].ToString());
            }
            return texto;
        }

        public static string NumeroFormatado(int numero, bool centena = false)
        {
            string zeros = "";
            zeros += (centena && numero < 100) ? "0" : "";
            zeros += numero < 10 ? "0" : "";
            return zeros + numero;
        }

        public static string GetEnumDescription(Enum? value)
        {
            if(value == null) return string.Empty;

            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =(DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }
}