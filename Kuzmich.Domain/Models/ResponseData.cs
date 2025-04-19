using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kuzmich.Domain.Models
{
    public class ResponseData<T> // Класс, описывающий формат данных, получаемых от сервисов
    {
        // запрашиваемые данные
        public T Data { get; set; }
        // признак успешного завершения запроса
        public bool Success { get; set; } = true;
        // сообщение в случае неуспешного завершения
        public string? ErrorMessage { get; set; }
    }
}
