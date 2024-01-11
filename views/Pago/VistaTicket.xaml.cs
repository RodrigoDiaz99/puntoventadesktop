using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace punto_venta.views.Pago
{
    /// <summary>
    /// Lógica de interacción para VistaTicket.xaml
    /// </summary>
    public partial class VistaTicket : Window
    {
        private Pagos pagos;
        public VistaTicket(Pagos pagos)
        {
            InitializeComponent();
            this.pagos = pagos;
        }
    }
}
