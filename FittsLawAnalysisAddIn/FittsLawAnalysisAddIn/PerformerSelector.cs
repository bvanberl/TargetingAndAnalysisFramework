using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using ScenarioSim.Core.Entities;
using ScenarioSim.Core.Interfaces;
using ScenarioSim.Infrastructure.JsonNetSerializer;

namespace FittsLawAnalysisAddIn
{
    public partial class PerformerSelector : Form
    {
        public IEnumerable<Performer> performers;
        public SetPerformersDelegate perfDelegate;
        public event SetPerformersDelegate OnSendMessage;

        public PerformerSelector()
        {

            InitializeComponent();

            using (HttpClient client = new HttpClient())
            {
                var task = client.GetAsync($"http://scenariosim.azurewebsites.net/api/performer");
                string result = task.Result.Content.ReadAsStringAsync().Result;
                performers = JsonNetSerializer.DeserializeObject<IEnumerable<Performer>>(result);
            }

            // Add the performers to the checked box list
            if (performers != null)
            {
                using (var enr = performers.GetEnumerator())
                {
                    while (enr.MoveNext())
                    {
                        performer_list.Items.Add(enr.Current.Name, 0);
                    }
                }
            }
        }

        private void return_button_Click(object sender, EventArgs e)
        {
            // Get information on all selected performers
            List<Performer> selPerformers = new List<Performer>();
            if (performers != null)
            {
                foreach (object chk in performer_list.CheckedItems)
                {
                    using (var enr = performers.GetEnumerator())
                    {
                        while (enr.MoveNext())
                        {
                            string thgrfd = chk.ToString();
                            if (chk.ToString() == enr.Current.Name)
                            {
                                selPerformers.Add(enr.Current);
                            }
                        }
                    }
                }

            }

            // Call the import_performer_data method in the ribbon
            OnSendMessage?.Invoke(selPerformers);

            // Close this form
            this.Close();
        }
    }
}
