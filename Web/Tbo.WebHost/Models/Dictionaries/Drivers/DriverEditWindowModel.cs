namespace Tbo.WebHost.Models.Dictionaries.Drivers
{
    /// <summary>
    /// Модель для открытия окна создания/редактирвоания водителя
    /// </summary>
    /// <typeparam name="T">тип dto</typeparam>
    public class DriverEditWindowModel<T> where T : class
    {
        /// <summary>
        /// Данные
        /// </summary>
        public T Data { get; set; }
        
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="data">dto</param>
        /// <param name="cars">автомобили
        public DriverEditWindowModel(T data)
        {
            Data = data;
        }
    }
}