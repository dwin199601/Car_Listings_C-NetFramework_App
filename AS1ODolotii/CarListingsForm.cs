using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Globalization;
using System.Linq.Expressions;

namespace AS1ODolotii
{
    public partial class CarListingsForm : Form
    {
        public List<Car> CarList;
       

        #region Method to Read data from the xml file 
        public void OpenFile()
        {

            // Display an OpenFileDialog 
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "XML Files (*.xml)|*.xml",
                Title = @"Select a XML File",
                FilterIndex = 0,
                DefaultExt = "xml"
            };

            if (openFileDialog.ShowDialog() != DialogResult.OK) 
                return;
            try
            {
                var reader = new StreamReader(openFileDialog.FileName);
                //create the serializer
                var carSerializer = new XmlSerializer(typeof(List<Car>));
                //deserialize to the list
                CarList = carSerializer.Deserialize(reader) as List<Car>;
                reader.Close(); // close the file after deserializing

                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid file type! " + ex);
            }

        }

        #endregion

        public CarListingsForm()
        {
            InitializeComponent();
            OpenFile();
          
            Text = "Assignment 1 – Car Listings";
            //LINQ statement to order carList members: Make (asc order), Price(asc order), Year(desc order), Color(asc order)
            var orderList = (from car in CarList
                             orderby car.Make ascending, car.Price ascending, car.Year descending, car.Color ascending
                             select car).ToList();

              GriedViewSetting(AllCardataGridView);
              GriedViewSetting(SelectedCarsdataGridView);

           
            CountAvgSet(CarList); // method to populate Count and Avg price textBoxes
            AddDataToListBoxes(CarList);//adding data from the carList to the ListBoxes

            
           // LabelStylesHere(Filterlabel);
            Filterlabel.Text = "Filters";
            LabelStylesHere(Filterlabel);
            LabelStylesHere(AllCarslabel);
            LabelStylesHere(SelectCarlabel);


            ListBoxSettings(MakeslistBox);
            ListBoxSettings(YearslistBox);
            ListBoxSettings(ColorslistBox);
            ListBoxSettings(DealerslistBox);
            TextBoxSetting(CounttextBox);
            TextBoxSetting(AvgPriceTextBox);
            TextBoxSetting(MinPricetextBox);
            TextBoxSetting(MaxtextBox);
            TextBoxSetting(MinSizetextBox);
            TextBoxSetting(MaxEngineSizetextBox);
            Resetbutton.Click += ButtonReset_Click;
            PricecheckBox.CheckedChanged += PriceCheckBox_Changed;
            EngineSizecheckBox.CheckedChanged += EngineSizeCheckBox_Changed;
            MinPricetextBox.TextChanged += MinPrice_TextChange;
            MaxtextBox.TextChanged += MaxPrice_TextChange;
            MinSizetextBox.TextChanged += MinSize_TextChange;
            MaxEngineSizetextBox.TextChanged += MaxSize_TextChange;

            TopDataGridViewMethod();
            AllCardataGridView.Columns[4].DefaultCellStyle.Format = "c2";
        }

        private void TopDataGridViewMethod()
        {
           
            AllCardataGridView.Rows.Clear();
            foreach (var car in from car in CarList
                                orderby car.Make
                                ascending, car.Price ascending,
                                car.Year descending,
                                car.Color ascending
                                select car) 
            {

                AllCardataGridView.Rows.Add(car.Make, car.Year,
                    car.Color, car.EngineSize, car.Price, car.Dealer);
             }

        }

        private void MaxSize_TextChange(object sender, EventArgs e)
        {
            if (decimal.TryParse(MaxEngineSizetextBox.Text, out var maxSize))
            {
                DisplaySelected();
            }
            else
            {
                MessageBox.Show(this, @"Max Size is missing or it's not a decimal !!", @"Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                EngineSizecheckBox.Checked = false;
                MaxEngineSizetextBox.Focus();
                DisplaySelected();
            }
        }

        private void MinSize_TextChange(object sender, EventArgs e)
        {
            if (decimal.TryParse(MinSizetextBox.Text, out var minSize))
            {
                DisplaySelected();
            }
            else
            {
                MessageBox.Show(this, @"Min Size is missing or it's not a decimal!!", @"Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                EngineSizecheckBox.Checked = false;
                MinSizetextBox.Focus();
                DisplaySelected();
            }
        }

        private void MaxPrice_TextChange(object sender, EventArgs e)
        {
            if (int.TryParse(MaxtextBox.Text, out var maxPrice))
            {
                DisplaySelected();
            }
            else
            {
                MessageBox.Show(this, @"Price is missing or it's not integer!!", @"Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                PricecheckBox.Checked = false;
                MaxtextBox.Focus();
                DisplaySelected();
            }
        }

        private void MinPrice_TextChange(object sender, EventArgs e)
        {
            if (int.TryParse(MinPricetextBox.Text, out var minPrice))
            {
                DisplaySelected();
            }
            else
            {
                MessageBox.Show(this, @"Price is missing or it's not integer!!", @"Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                PricecheckBox.Checked = false;
                MinPricetextBox.Focus();
                DisplaySelected();
            }

        }

        private void EngineSizeCheckBox_Changed(object sender, EventArgs e)
        {
            try
            {
                DisplaySelected();
            }

            catch (Exception)
            {
                MessageBox.Show(this, @"Enigne Size is missing or it's not decimal!!!", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                PricecheckBox.Checked = false;
            }
        }

        private void PriceCheckBox_Changed(object sender, EventArgs e)
        {
           
            try
            {
                DisplaySelected();
            }
            catch (Exception)
            {
                MessageBox.Show(this, @"Price is missing or it's not an intager!!", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                PricecheckBox.Checked = false;
               
            }
        }


        #region Styles for labels
        private static void LabelStylesHere(Label labelName)
        {
            labelName.BorderStyle = BorderStyle.FixedSingle;
            labelName.Padding = new Padding(2);
            labelName.Font = new Font("Calibri", 10);

            labelName.ForeColor = Color.White;
            labelName.BackColor = Color.Blue;
        }
        #endregion
        #region TextBox Settings
        public static void TextBoxSetting(TextBox textBoxName)
        {
            textBoxName.Width = 100;
            textBoxName.Height = 20;
            
        }

        #endregion
        #region ListBox Settings
        /// <summary>
        /// Settings for ListBoxes
        /// </summary>
        /// <param name="listBoxName"></param>
        public static void ListBoxSettings(ListBox listBoxName)
        {
            listBoxName.Height = 100;
            listBoxName.SelectionMode = SelectionMode.MultiExtended;
     

        }
        #endregion

        #region DataGriedView controls settings
        /// <summary>
        /// All setting for the DataGriedView controls
        /// </summary>
        /// <param name="gridViewName"></param>
        public static void GriedViewSetting(DataGridView gridViewName)
        {
            gridViewName.ReadOnly = true;
            
            

            gridViewName.AllowUserToAddRows = false;
            gridViewName.AllowUserToDeleteRows = false;
            gridViewName.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            gridViewName.RowHeadersVisible = false;
            gridViewName.AutoSize = false;
            gridViewName.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            gridViewName.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            gridViewName.Width = 600;
            gridViewName.Height = 150;


            //need to clear datagriedviewbox columns before adding new
            gridViewName.Columns.Clear();
            //let's create column for the top datagriedview box
            DataGridViewColumn[] columns =
            {
                new DataGridViewTextBoxColumn(){Name = "Make"},
                new DataGridViewTextBoxColumn(){Name = "Year"},
                new DataGridViewTextBoxColumn(){Name = "Color"},
                new DataGridViewTextBoxColumn(){Name = "Engine\nSize"},
                new DataGridViewTextBoxColumn(){Name = "Price"},
                new DataGridViewTextBoxColumn(){Name = "Dealer"}

            };
            gridViewName.Columns.AddRange(columns);


        }
        #endregion

        #region The method to insert data into the listboxes by using LINQ quaries
        public void AddDataToListBoxes(List<Car> carList)
        {

            var makes = from carHere in carList
                        group carHere by carHere.Make into groups
                        orderby groups.Key
                        select groups.Key;

            MakeslistBox.DataSource = makes.ToList();

            SelectAllListBoxItems(MakeslistBox);

             var years = from carHere in carList
                        group carHere by carHere.Year into groups
                        orderby groups.Key
                        select groups.Key;
            YearslistBox.DataSource = years.ToList();
            SelectAllListBoxItems(YearslistBox);

            var colors = from carHere in carList
                         group carHere by carHere.Color into groups
                        orderby groups.Key
                         select groups.Key;
            ColorslistBox.DataSource = colors.ToList();
            SelectAllListBoxItems(ColorslistBox);

            var dealers = from carHere in carList
                          group carHere by carHere.Dealer into groups
                          orderby groups.Key
                          select groups.Key;
            DealerslistBox.DataSource = dealers.ToList();
            SelectAllListBoxItems(DealerslistBox);


        }
        #endregion

        /// <summary>
        /// Method to sellect all items in the listBox, it's used inside of AddDataToListBoxes method 
        /// </summary>
        /// <param name="listName"></param>
        private void SelectAllListBoxItems(ListBox listName)
        {
            for (int i = 0; i < listName.Items.Count; i++)
            {
                listName.SetSelected(i, true);
            }
        }

        #region Method to set value into the Count and Avg price TextBoxes(for the top box datagridview)
        private void CountAvgSet(List<Car>ListName)
        {
          
            int numberItems = ListName.Count();
            CounttextBox.Text = numberItems.ToString();
            var avg = ListName.Select(x => x.Price).Average();
           
            AvgPriceTextBox.Text = avg.ToString("C", CultureInfo.CurrentCulture);
           
        }

        #endregion

        #region Reset button method (to clear all text boxes and the bottom datagridview box)

        private void ButtonReset_Click(object sender, EventArgs e)
        {
            PricecheckBox.Checked = false;
            EngineSizecheckBox.Checked = false;
            MinPricetextBox.Text = "";
            MaxtextBox.Text = "";
            MinSizetextBox.Text = "";
            MaxEngineSizetextBox.Text = "";
            SelectedCarsdataGridView.Rows.Clear();

        }
        #endregion
        #region Main filter method 
        /// <summary>
        /// This method includes all activities for filling up the bottom DataGridView control with filtered data
        /// </summary>
        public void DisplaySelected()
        {
            //need to clear the griedview first before doing any filttering
            SelectedCarsdataGridView.Rows.Clear();
           
           
            int.TryParse(MinPricetextBox.Text, out var minPrice);
            int.TryParse(MaxtextBox.Text, out  var maxPrice);
            decimal.TryParse(MinSizetextBox.Text, out var minEngSize);
            decimal.TryParse(MaxEngineSizetextBox.Text, out var maxEngSize);

            //Here I've created lists of chosen makes,years,colors and dealers
            List<String> makesSelected = (from object selected
                                       in MakeslistBox.SelectedItems
                                        select selected.ToString()).ToList();

            List<String> yearsSelected = (from object selected in YearslistBox.SelectedItems
                                          select selected.ToString()).ToList();

            List<String> colorsSelected = (from object selected in ColorslistBox.SelectedItems
                                           select selected.ToString()).ToList();

            List<String> dealersSelected = (from object selected in DealerslistBox.SelectedItems
                                            select selected.ToString()).ToList();

            //building a LINQ query filtering by enigine size and price
            var allSelected = from car in CarList
                             from makeName in makesSelected
                             where makeName == car.Make
                             from yearCar in yearsSelected
                             where yearCar == car.Year.ToString()
                             from colorCar in colorsSelected
                             where colorCar == car.Color
                             from dealerCar in dealersSelected
                             where dealerCar == car.Dealer
                             where (car.Price >= minPrice && car.Price <= maxPrice ||!PricecheckBox.Checked) &&

                             (car.EngineSize >= minEngSize && car.EngineSize <= maxEngSize || !EngineSizecheckBox.Checked)
                             orderby car.Make
                                ascending, car.Price ascending,
                                car.Year descending,
                                car.Color ascending

                              select car;
           
            //Adding all car elements to the bottom grid view 
            foreach(var carElement in allSelected)
            {
                SelectedCarsdataGridView.Rows.Add(carElement.Make, carElement.Year,
                    carElement.Color, carElement.EngineSize, carElement.Price, carElement.Dealer);
                //formating price column 
               

            }
            SelectedCarsdataGridView.Columns[4].DefaultCellStyle.Format = "c2";

            //Here I've done calculation and found avgPrice of all present cars in the DataGridView and counted the number of all elements 
            int numItems = allSelected.Count();
            CountSelCarstextBox.Text = numItems.ToString();
            var avgPrice = allSelected.Select(x => x.Price).Average();
            AvgPriceSelCarstextBox.Text = avgPrice.ToString("C", CultureInfo.CurrentCulture);


        }
        #endregion

    }
}
