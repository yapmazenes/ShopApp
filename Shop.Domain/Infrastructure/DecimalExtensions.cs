using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Infrastructure
{
    public static class DecimalExtensions
    {
        public static string GetValueString(this decimal value) =>
                $"{value.ToString("N2")} ₺";


    }
}
