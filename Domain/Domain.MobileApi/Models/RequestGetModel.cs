namespace Domain.MobileApi.Models
{
    /// <summary>
    /// Детальная информация о заявке
    /// </summary>
    public class RequestGetModel
    {
        /// <summary>
        /// Идентификатор заявки
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// Тип контейнера
        /// </summary>
        public string ContainerType { get; set; }

        /// <summary>
        /// Способ оплаты
        /// </summary>
        public string PaymentType { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public decimal Sum { get; set; }

        /// <summary>
        /// Статус заявки
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Тип заявки
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Заказчик
        /// </summary>
        public RequestCustomerModel Customer { get; set; }

        /// <summary>
        /// Заказчик
        /// </summary>
        public RequestPolygonModel Polygon { get; set; }
    }

    /// <summary>
    /// Информация о заказчике
    /// </summary>
    public class RequestCustomerModel
    {
        /// <summary>
        /// Имя контактного лица
        /// </summary>
        public string ContactPersonName { get; set; }

        /// <summary>
        /// Телефон контактного лица
        /// </summary>
        public string ContactPersonPhone { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Дополнительный номер телефона
        /// </summary>
        public string AdditionalPhoneNumber { get; set; }

        /// <summary>
        /// Широта
        /// </summary>
        public decimal? Latitude { get; set; }

        /// <summary>
        /// Долгота
        /// </summary>
        public decimal? Longitude { get; set; }
    }

    /// <summary>
    /// Информация о полигоне
    /// </summary>
    public class RequestPolygonModel
    {
        /// <summary>
        /// Наименование полигона
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Номер телефона полигона
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Адрес полигона
        /// </summary>
        public string Address { get; set; }
    }
}
