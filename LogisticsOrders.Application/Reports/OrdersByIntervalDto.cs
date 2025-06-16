using System.Collections.Generic;

namespace LogisticsOrders.Application.Reports
{
    /// <summary>
    /// DTO para reportar la cantidad de órdenes por cliente y por intervalo de distancia.
    /// </summary>
    public class OrdersByIntervalDto
    {
        /// <summary>
        /// Nombre del cliente.
        /// </summary>
        public string Client { get; set; } = string.Empty;

        /// <summary>
        /// Lista de intervalos y su cantidad de órdenes.
        /// </summary>
        public List<IntervalCount> Intervals { get; set; } = new();

        /// <summary>
        /// Representa un intervalo de distancia y la cantidad de órdenes en ese intervalo.
        /// </summary>
        public class IntervalCount
        {
            /// <summary>
            /// Etiqueta del intervalo (ej: "1-50 km").
            /// </summary>
            public string Interval { get; set; } = string.Empty;

            /// <summary>
            /// Cantidad de órdenes en este intervalo.
            /// </summary>
            public int Count { get; set; }
        }
    }
}