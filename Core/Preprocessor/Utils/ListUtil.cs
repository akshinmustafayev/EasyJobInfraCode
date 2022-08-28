using System.Collections.Generic;

namespace EasyJobInfraCode.Core.Preprocessor.Utils
{
    public static class ListUtil
    {
        public static string ConvertListToString(List<object> listToConvert, string qotationMark = "\"", string delimiter = " ")
        {
            string data = "";

            if (listToConvert.Count > 0)
            {
                for (int i = 0; i < listToConvert.Count; i += 1)
                {
                    if (i == listToConvert.Count - 1)
                        data = data + qotationMark + listToConvert[i].ToString() + qotationMark;
                    else
                        data = data + qotationMark + listToConvert[i].ToString() + qotationMark + delimiter;
                }
            }

            return data;
        }
    }
}
