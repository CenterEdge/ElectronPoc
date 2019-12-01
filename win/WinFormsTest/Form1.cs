using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace WinFormsTest
{
    public partial class Form1 : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private ProcessManager _processManager;

        public Form1()
        {
            InitializeComponent();

            var services = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder.AddDebug();
                })
                .AddSingleton<ProcessManager>();

            _serviceProvider = services.BuildServiceProvider();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            _processManager = _serviceProvider.GetRequiredService<ProcessManager>();

            await _processManager.StartAsync(CancellationToken.None);

            btnShow.Enabled = true;
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            _processManager.MessagePipe.ObserveOn(SynchronizationContext.Current).Take(1).Subscribe(async pipe =>
            {
                var result = await new ModalForm(pipe).ShowDialog(this, "page-one");

                MessageBox.Show(this, result, "Result");
            });
        }
    }
}
