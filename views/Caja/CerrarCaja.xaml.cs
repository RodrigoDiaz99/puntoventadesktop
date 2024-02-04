using punto_venta.models;
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

namespace punto_venta.views.Caja
{
    /// <summary>
    /// Lógica de interacción para CerrarCaja.xaml
    /// </summary>
    public partial class CerrarCaja : Window
    {
        private int id_corte_cajas;
        private int userId;
        private Venta venta;
        public CerrarCaja(Venta ventaIn, int userId, int id_corte_cajas)
        {
            InitializeComponent();
            this.id_corte_cajas = id_corte_cajas;
            this.userId = userId;
            venta = ventaIn;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new DBConnection())
                {
                    double dEfectivoInicial = 0;
                    double dTotalTarjeta = 0;
                    double dEfectivoEntrante = 0;
                    double dEfectivoSaliente = 0;
                    double dEfectivoFinal = 0;
                    double dEfectivoCalculado = 0;
                    double dEfectivoFaltante = 0;
                    double dEfectivoSobrante = 0;
                    double dEfectivoDiferencia = 0;


                    bool esFaltante = false;

                    List<vouchers> lstVouchers = (from x in context.vouchers
                                                  where x.corte_cajas_id == id_corte_cajas
                                                  select x).ToList();

                    Corte_Caja corte_cajas = (from x in context.corte_cajas
                                              where x.id == id_corte_cajas
                                              select x).FirstOrDefault();

                    dEfectivoInicial = corte_cajas.cantidad_inicial;

                    if (corte_cajas == null)
                    {
                        throw new Exception("Ocurrió un problema al obtener la información de la caja.");
                    }

                    foreach (var voucher in lstVouchers)
                    {
                        dEfectivoEntrante += voucher.cantidad_pagada_efectivo;
                        dTotalTarjeta += voucher.cantidad_pagada_tarjeta;
                        dEfectivoSaliente += voucher.cambio_efectivo;
                    }

                    dEfectivoCalculado = dEfectivoInicial + dEfectivoEntrante - dEfectivoSaliente;

                    dEfectivoFinal = Convert.ToDouble(this.dEfectivoFinal.Text);
                    dEfectivoDiferencia = dEfectivoFinal - dEfectivoCalculado;

                    if (dEfectivoDiferencia < 0)
                    {
                        dEfectivoFaltante = dEfectivoDiferencia;
                    }
                    else
                    {
                        dEfectivoSobrante = dEfectivoDiferencia;
                    }

                    ResultadoCaja resultadoCaja = new ResultadoCaja(userId, id_corte_cajas, dEfectivoInicial, dEfectivoEntrante, dEfectivoSaliente, dEfectivoFinal, dEfectivoFaltante, dEfectivoSobrante);

                    this.Close();
                    venta.btnAgregarProducto.IsEnabled = false;
                    venta.gridAccionesCajaAbierta.Visibility = Visibility.Hidden;
                    venta.gridAccionesCajaCerrada.Visibility = Visibility.Visible;
                    corte_cajas.lActivo = false;
                    context.SaveChanges();
                    resultadoCaja.ShowDialog();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un problema: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
