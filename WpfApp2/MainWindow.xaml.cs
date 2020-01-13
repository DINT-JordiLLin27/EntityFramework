using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        private CLIENTE nuevoCliente;
        private InformesEntities contexto;
        private CollectionViewSource vista;
        public MainWindow()
        {
            InitializeComponent();

            contexto = new InformesEntities();
            nuevoCliente = new CLIENTE();
            vista = new CollectionViewSource();

            contexto.CLIENTES.Load();
            vista.Source = contexto.CLIENTES.Local;

            this.DataContext = contexto.CLIENTES.Local;

            //DatosListBox.DataContext = contexto.CLIENTES.Local;
            InsertarStackPanel.DataContext = nuevoCliente;
            FiltrarDatosDataGrid.DataContext = vista;
            vista.Filter += Vista_Filter;
            //EliminarDatosComboBox.DataContext = contexto.CLIENTES.Local;
            //ModificarStackPanel.DataContext = contexto.CLIENTES.Local;
        }

        private void InsertarButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.Parse(IdentificadorTextBox.Text) > contexto.CLIENTES.Count())
            {
                contexto.CLIENTES.Add(nuevoCliente);
                contexto.SaveChanges();
                nuevoCliente = new CLIENTE();
                InsertarStackPanel.DataContext = nuevoCliente;
            }
            else MessageBox.Show("El campo 'Identificador' ya existe", "Insertar Dato", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void EliminarButton_Click(object sender, RoutedEventArgs e)
        {
            CLIENTE deleteCliente = (CLIENTE)EliminarDatosComboBox.SelectedItem;

            MessageBoxResult boxResult = MessageBox.Show("Eliminar " + deleteCliente.nombre + " " + deleteCliente.apellido + "?", "Eliminar Dato", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);

            if (boxResult == MessageBoxResult.OK)
            {
                contexto.CLIENTES.Remove((CLIENTE)EliminarDatosComboBox.SelectedItem);
                contexto.SaveChanges();
            }
            else MessageBox.Show("No se ha eliminado " + deleteCliente.nombre + " " + deleteCliente.apellido, "Eliminar Dato", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ModificarButton_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult boxResult = MessageBox.Show("¿Actualizar " + ((CLIENTE)ModificarDatosComboBox.SelectedItem).nombre + " " + ((CLIENTE)ModificarDatosComboBox.SelectedItem).apellido + "?", "Actualizar Dato", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);

            if (boxResult == MessageBoxResult.OK)
            {
                contexto.SaveChanges();
                MessageBox.Show("Actualizado correctamente", "Actualizar Dato", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else MessageBox.Show("No se ha actualizado " + ((CLIENTE)ModificarDatosComboBox.SelectedItem).nombre + " " + ((CLIENTE)ModificarDatosComboBox.SelectedItem).apellido, "Actualizar Dato", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ActualizarDataGridButton_Click(object sender, RoutedEventArgs e)
        {
            contexto.SaveChanges();
        }


        private void Vista_Filter(object sender, FilterEventArgs e)
        {
            CLIENTE item = (CLIENTE)e.Item;
            
            if (FiltroTextBox.Text == "")
            {
                e.Accepted = true;
            }
            else
            {
                if (item.nombre.Contains(FiltroTextBox.Text) || item.apellido.Contains(FiltroTextBox.Text))
                {
                    e.Accepted = true;
                }
                else
                {
                    e.Accepted = false;
                }
            }
        }

        private void FiltrarButton_Click(object sender, RoutedEventArgs e)
        {
            vista.View.Refresh();
        }
    }
}
