using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AccountBook
{
    public class AccountService
    {
        private List<Record> Records;
        private int NextId;
        private string LocalPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "/TestCSharp/account_book.txt");

        public AccountService()
        {
            Records = new List<Record>();
            NextId++;
        }

        #region 数据处理
        public void AddRecord(DateTime date, string type, string category, double amount, string note)
        {
            var record = new Record
            {
                Id = NextId++,
                Date = date,
                Type = type,
                Category = category,
                Amount = amount,
                Note = note
            };
            Records.Add(record);
            Console.WriteLine($"✅ 添加成功！记录编号：{record.Id}");

            SaveData();
        }

        public void DeleteRecord(int id)
        {
            var record = Records.FirstOrDefault(r => r.Id == id);
            if(record == null)
            {
                Console.WriteLine("❌ 未找到该记录");
                return;
            }

            Records.Remove(record);
            Console.WriteLine($"✅ 已删除记录：{record.Date:yyyy-MM-dd} {record.Category} {record.Amount}元");

            SaveData();
        }

        public void UpdateRecord(int id, DateTime date, string type, string category, double amount, string note)
        {
            var record = Records.FirstOrDefault(r => r.Id == id);
            if(record == null)
            {
                Console.WriteLine("❌ 未找到该记录");
                return;
            }

            record.Date = date;
            record.Type = type;
            record.Category = category;
            record.Amount = amount;
            record.Note = note;

            Console.WriteLine($"✅ 已修改记录：{id}");

            SaveData();
        }

        public void ShowAllRecords()
        {
            if (Records.Count == 0)
            {
                Console.WriteLine("📭 暂无记账记录");
                return;
            }

            Console.WriteLine("\n========== 所有记账记录 ==========");
            Console.WriteLine("编号\t日期\t类型\t分类\t金额\t备注");
            Console.WriteLine("------------------------------------");

            foreach (var record in Records.OrderByDescending(r => r.Date))
            {
                if (record.Type == "收入")
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else // 支出
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                Console.WriteLine($"{record.Id}\t{record.Date:yyyy-MM-dd}\t{record.Type}\t{record.Category}\t{record.Amount:F2}\t{record.Note}");
                Console.ResetColor();
                Console.WriteLine();
            }
        }

        public void ShowStatisticsByMonth(int year, int month)
        {
            var monthRecords = Records.Where(r => r.Date.Year == year && r.Date.Month == month).ToList();
            if(monthRecords.Count == 0)
            {
                Console.WriteLine($"📭 {year}年{month}月 无记账记录");
                return;
            }

            var monthlyIncome = monthRecords.Where(r => r.Type == "收入").Sum(w => w.Amount);
            var monthlyExpense = monthRecords.Where(r => r.Type == "支出").Sum(w => w.Amount);
            var monthlyBalance = monthlyIncome - monthlyExpense;

            Console.WriteLine($"\n========== {year}年{month}月 统计 ==========");
            Console.WriteLine($"💰 总收入：{monthlyIncome:F2} 元");
            Console.WriteLine($"💸 总支出：{monthlyExpense:F2} 元");
            Console.WriteLine($"📊 结余：{(monthlyBalance >= 0 ? "+" : "")}{monthlyBalance:F2} 元");
        }
        #endregion

        #region 数据持久化
        private void SaveData()
        {
            try
            {
                var data = new { Records, NextId };
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true // 格式化输出
                };
                string json = JsonSerializer.Serialize(data, options);
                File.WriteAllText(LocalPath, json);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"⚠️ 保存失败：{ex.Message}");
            }
        }

        public void LoadData()
        {
            
        }
        #endregion
    }
}
