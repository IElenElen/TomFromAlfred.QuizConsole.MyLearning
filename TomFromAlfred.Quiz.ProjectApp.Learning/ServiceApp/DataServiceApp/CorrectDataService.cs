using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.DataServiceApp
{
    public class CorrectDataService
    {
        private List<ContentCorrectSet> ContentCorrectSets { get; set; }

        public CorrectDataService()
        {
            ContentCorrectSets = [];
            InitializeData(); 
        }

        public void InitializeData()
        {
            ContentCorrectSets.Add(new ContentCorrectSet("Z ilu części składa się powieść Alfreda Szklarskiego?", EntitySupport.OptionLetter.B, "9"));
            ContentCorrectSets.Add(new ContentCorrectSet("Jaki tytuł nosi ostatnia część o przygodach Tomka?", EntitySupport.OptionLetter.A, "Tomek w grobowcach faraonów."));
            ContentCorrectSets.Add(new ContentCorrectSet("Tomek przed pierwszą przygodą mieszka w: ", EntitySupport.OptionLetter.C, "W Warszawie."));
            ContentCorrectSets.Add(new ContentCorrectSet(" ", EntitySupport.OptionLetter.A, " "));
            ContentCorrectSets.Add(new ContentCorrectSet(" ", EntitySupport.OptionLetter.C, " "));
            ContentCorrectSets.Add(new ContentCorrectSet(" ", EntitySupport.OptionLetter.A, " "));
            ContentCorrectSets.Add(new ContentCorrectSet(" ", EntitySupport.OptionLetter.A, " "));
            ContentCorrectSets.Add(new ContentCorrectSet(" ", EntitySupport.OptionLetter.C, " "));
            ContentCorrectSets.Add(new ContentCorrectSet(" ", EntitySupport.OptionLetter.B, " "));
        }

        public void LoadDataFromJson(string filePath)
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                ContentCorrectSets = JsonSerializer.Deserialize<List<ContentCorrectSet>>(json) ?? [];
            }
            else
            {
                InitializeData();
            }
        }

        public void SaveDataToJson(string filePath)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(ContentCorrectSets, options);
            File.WriteAllText(filePath, json);
        }
    }
}
