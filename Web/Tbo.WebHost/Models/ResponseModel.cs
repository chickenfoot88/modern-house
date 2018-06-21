using Core.DataAccess.Extensions;
using Core.DataAccess.Params;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Tbo.WebHost.Models
{
    /// <summary>
    /// Модель ответа
    /// </summary>
    public class ResponseModel<T> : ResponseModel
    {
        /// <summary>
        /// Данные
        /// </summary>
        [JsonProperty("Data")]
        public T Data { get; set; }

        /// <summary>
        /// Успешный ответ с данными
        /// </summary>
        /// <param name="content"></param>
        public static ResponseModel<T> Ok(T content)
        {
            return new ResponseModel<T> { Success = true, Data = content };
        }
    }

    /// <summary>
    /// Модель ответа
    /// </summary>
    public class ListResult<T> : ResponseModel<List<T>>
    {
        /// <summary>
        /// Число отфильтрованных строк
        /// </summary>
        [JsonProperty("recordsFiltered")]
        public int RecordsFiltered { get; set; }

        /// <summary>
        /// Общее количество строк
        /// </summary>
        [JsonProperty("recordsTotal")]
        public int RecordsTotal { get; set; }

        /// <summary>
        /// Успешный ответ с данными
        /// </summary>
        /// <param name="content"></param>
        /// <param name="recordsFiltered"></param>
        /// <param name="recordsTotal"></param>
        public static ListResult<T> Ok(List<T> content, int recordsFiltered, int recordsTotal)
        {
            return new ListResult<T>
            {
                Success = true,
                Data = content,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        /// <summary>
        /// Успешный ответ с данными
        /// </summary>
        /// <param name="query"></param>
        /// <param name="loadParams"></param>
        public static ListResult<T> From(IQueryable<T> query, StoreLoadParams loadParams)
        {
            var total = query.Count();

            var filtered = query.Count();

            var data = query
                .Paging(loadParams)
                .ToList();

            return Ok(data, filtered, total);
        }
    }


    /// <summary>
    /// Модель ответа
    /// </summary>
    public class ResponseModel
    {
        /// <summary>
        /// Успешность
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Текст ошибки
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Успешный ответ
        /// </summary>
        public static ResponseModel Ok()
        {
            return new ResponseModel { Success = true };
        }

        /// <summary>
        /// Ответ содержащий ошибку
        /// </summary>
        /// <param name="error">текст ошибки</param>
        public static ResponseModel Failure(string error)
        {
            return new ResponseModel { Success = false, Error = error };
        }
    }
}