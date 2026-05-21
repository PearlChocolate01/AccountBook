using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountBook
{
    public class Record
    {
        /// <summary>
        /// 编号（自动生成）
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// 收入/支出
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 分类（餐饮、购物、工资等）
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public double Amount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Note { get; set; }

        public string BuildRecord()
        {
            return $"{Date:yyyy-MM-dd}\t{Type}\t{Category}\t{Amount:F2}\t{Note}";
        }
    }
}
