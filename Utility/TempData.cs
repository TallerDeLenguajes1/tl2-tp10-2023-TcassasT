using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace tl2_tp10_2023_TcassasT.Utility;

/*
  Crédito: https://stackoverflow.com/questions/34638823/store-complex-object-in-tempdata/35042391#35042391
*/

[Serializable]
public static class TempDataExtensions
{
  public static void Put<T>(this ITempDataDictionary tempData, string key, T value) where T : class
  {
    tempData[key] = JsonConvert.SerializeObject(value);
  }

  public static T Get<T>(this ITempDataDictionary tempData, string key) where T : class
  {
    object o;
    tempData.TryGetValue(key, out o);
    return o == null ? null : JsonConvert.DeserializeObject<T>((string)o);
  }
}
