﻿using System.Windows;
using System.Windows.Controls;
using DeLong_Desktop.ApiService.Helpers;
using DeLong_Desktop.ApiService.DTOs.Enums;
using DeLong_Desktop.ApiService.DTOs.Users;
using DeLong_Desktop.ApiService.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using DeLong_Desktop.ApiService.DTOs.Customers;
using System.Windows.Media;

namespace DeLong_Desktop.Windows.Customers;

/// <summary>
/// Interaction logic for CustomerAddWindow.xaml
/// </summary>
public partial class CustomerAddWindow : Window
{
    private readonly IUserService userService;
    private readonly ICustomerService customerService;
    private readonly IServiceProvider services;
    public bool IsCreated { get; set; } = false;

    string gender = "";
    string userJshshir = "";

    public CustomerAddWindow(IServiceProvider services)
    {
        InitializeComponent();
        this.services = services;
        this.userService = services.GetRequiredService<IUserService>();
        this.customerService = services.GetRequiredService<ICustomerService>();
    }
      
        // radio buttonlar
    private void rbtnYurdik_Checked(object sender, RoutedEventArgs e)
    {
        spYurCutomer.Visibility = Visibility.Visible;
        spJisCutomer.Visibility = Visibility.Hidden;
        spYattCutomer.Visibility = Visibility.Hidden;
        spYurYattJis.Visibility = Visibility.Visible;
        spQaytish.Visibility = Visibility.Hidden;
        spEmployee.Visibility = Visibility.Hidden;
    }
    private void rbtnYaTT_Checked(object sender, RoutedEventArgs e)
    {
        spYurCutomer.Visibility = Visibility.Hidden;
        spJisCutomer.Visibility = Visibility.Hidden;
        spYattCutomer.Visibility = Visibility.Visible;
        spYurYattJis.Visibility = Visibility.Visible;
        spQaytish.Visibility = Visibility.Hidden;
        spEmployee.Visibility = Visibility.Hidden;
    }
    private void rbtnJismoniy_Checked(object sender, RoutedEventArgs e)
    {
        spYurCutomer.Visibility = Visibility.Hidden;
        spJisCutomer.Visibility = Visibility.Visible;
        spYattCutomer.Visibility = Visibility.Hidden;
        spYurYattJis.Visibility = Visibility.Visible;
        spQaytish.Visibility = Visibility.Hidden;
        spEmployee.Visibility = Visibility.Hidden;
    }
    private void rbtnEmployee_Checked(object sender, RoutedEventArgs e)
    {
        spYurCutomer.Visibility = Visibility.Hidden;
        spJisCutomer.Visibility = Visibility.Hidden;
        spYattCutomer.Visibility = Visibility.Hidden;
        spYurYattJis.Visibility = Visibility.Visible;
        spQaytish.Visibility = Visibility.Hidden;
        spEmployee.Visibility = Visibility.Visible;
    }
    private void RadioButton_Checked(object sender, RoutedEventArgs e)
    {
        RadioButton selectedRadioButton = sender as RadioButton;
        if (selectedRadioButton != null)
        {
            gender = selectedRadioButton.Content.ToString(); // Tanlangan matn: "Erkak" yoki "Ayol"
        }
        else
        {
            MessageBox.Show("Jinsini tanglang iltimos.");
            return;
        }

    }

        
        // buttonlar 
    private void btnJisAdd_Click(object sender, RoutedEventArgs e)
    {
        UserCreationDto userCreationDto = new UserCreationDto();

        userCreationDto.LastName = txtFamiliya.Text.ToLower();
        userCreationDto.FirstName = txtIsmi.Text.ToLower();
        userCreationDto.Patronomyc = txtSharifi.Text.ToLower();
        userCreationDto.SeriaPasport = txtPasportSeria.Text.ToLower();
        userCreationDto.Address = txtJisAdres.Text.ToLower();
        userCreationDto.Phone = txtJisTelefon.Text;
        userCreationDto.TelegramPhone = txtJisTelegramRaqam.Text;
        userCreationDto.JSHSHIR = txtJisJSHSHIR.Text;
        userJshshir = txtJisJSHSHIR.Text;


        if (!dateOfBirthPicker.SelectedDate.HasValue)
        {
            MessageBox.Show("Tug'ulgan sanani kiriting iltimos.");
            return;
        }
        userCreationDto.DateOfBirth = dateOfBirthPicker.SelectedDate.Value.ToUniversalTime();

        if (!dateOfIssuePicker.SelectedDate.HasValue)
        {
            MessageBox.Show("Pasport berilgan sanani kiriting iltimos.");
            return;
        }

        userCreationDto.DateOfIssue = dateOfIssuePicker.SelectedDate.Value.ToUniversalTime();
        if (!dateOfExpiryPicker.SelectedDate.HasValue)
        {
            MessageBox.Show("Pasportni amal qilish sanani kiriting iltimos.");
            return;
        }
        userCreationDto.DateOfExpiry = dateOfExpiryPicker.SelectedDate.Value.ToUniversalTime();
        if (gender.Equals("Erkak"))
            userCreationDto.Gender = (Gender)0;
        else if (gender.Equals("Ayol"))
            userCreationDto.Gender = (Gender)1;


        if (userCreationDto.FirstName.Equals("") ||
            userCreationDto.LastName.Equals("") ||
            userCreationDto.SeriaPasport.Equals("") ||
            userCreationDto.Address.Equals("") ||
            userCreationDto.JSHSHIR.Equals("") ||
            userCreationDto.TelegramPhone.Equals("") ||
            userCreationDto.Phone.Equals(""))

            MessageBox.Show("Malumotni to'liq kiriting!");
        else
        {
            var result = this.userService.AddAsync(userCreationDto);

            if (!result.IsCompletedSuccessfully)
            {
                MessageBox.Show($" Saqlandi.");
                IsCreated = true;
            }
            else
                MessageBox.Show($"{"Saqlashda xatolik"}");
        }
    }
    private async void btnYurAdd_Click(object sender, RoutedEventArgs e)
    {
        CustomerCreationDto customerCreationDto = new CustomerCreationDto();

        customerCreationDto.Name = txtYurNomi.Text.ToLower();
        if (txtYurINN.Text.Length != 9)
        {
            MessageBox.Show("INN kiriting iltimos.");
            return;
        }
        customerCreationDto.INN = int.Parse(txtYurINN.Text);
        customerCreationDto.MFO = txtYurMFO.Text;

        if (txtYurXisobRaqam.Text.Length != 23)
        {
            MessageBox.Show("Xisob raqamni to'liq kiriting iltimos.");
            return;
        }
        customerCreationDto.BankAccount = RemoveDashes(txtYurXisobRaqam.Text);
        customerCreationDto.Phone = txtYurPhone.Text;
        customerCreationDto.BankName = txtYurBank.Text.ToLower();
        customerCreationDto.OKONX = txtYurOKONX.Text;
        customerCreationDto.YurAddress = txtYurFirmaAdres.Text.ToLower();

        var user = await userService.RetrieveByJSHSHIRAsync(userJshshir);
        if (user is null)
        {
            MessageBox.Show("Rahbar ma'lumotini kiriting.");
            return;
        }
        customerCreationDto.UserId = user.Id;

        if (customerCreationDto.Name.Equals("") ||
           customerCreationDto.INN.Equals("") ||
           customerCreationDto.Phone.Equals("") ||
           customerCreationDto.MFO.Equals("") ||
           customerCreationDto.BankName.Equals("") ||
           customerCreationDto.OKONX.Equals("") ||
           customerCreationDto.YurAddress.Equals(""))

            MessageBox.Show("Ma'lumotni to'liq kiriting!");
        else
        {
            var result = this.customerService.AddAsync(customerCreationDto);

            if (!result.IsCompletedSuccessfully)
            {
                MessageBox.Show($" Saqlandi.");
                IsCreated = true;
            }
            else
                MessageBox.Show($"{"Saqlashda xatolik"}");
        }
    }
    private void btnRahbar_Click(object sender, RoutedEventArgs e)
    {
        spYurYattJis.Visibility = Visibility.Hidden;
        spYurCutomer.Visibility = Visibility.Hidden;
        spJisCutomer.Visibility = Visibility.Hidden;
        spYattCutomer.Visibility = Visibility.Hidden;
        spQaytish.Visibility = Visibility.Visible;
        spJisNew.Visibility = Visibility.Visible;
    }
    private void btnQaytish_Click(object sender, RoutedEventArgs e)
    {
        if (rbtnYurdik.IsChecked.Equals(true))
        {
            spYurCutomer.Visibility = Visibility.Visible;
            spJisCutomer.Visibility = Visibility.Hidden;
            spYattCutomer.Visibility = Visibility.Hidden;
            spYurYattJis.Visibility = Visibility.Visible;
            spQaytish.Visibility = Visibility.Hidden;
            spJisNew.Visibility = Visibility.Hidden;
        }
        else if (rbtnYaTT.IsChecked.Equals(true))
        {
            spYurCutomer.Visibility = Visibility.Hidden;
            spJisCutomer.Visibility = Visibility.Hidden;
            spYattCutomer.Visibility = Visibility.Visible;
            spYurYattJis.Visibility = Visibility.Visible;
            spQaytish.Visibility = Visibility.Hidden;
            spJisNew.Visibility = Visibility.Hidden;
        }
    }
    private void btnNewAdd_Click(object sender, RoutedEventArgs e)
    {
        spYurCutomer.Visibility = Visibility.Hidden;
        spJisCutomer.Visibility = Visibility.Visible;
        spYattCutomer.Visibility = Visibility.Hidden;
        spYurYattJis.Visibility = Visibility.Hidden;
        spQaytish.Visibility = Visibility.Visible;
        spJisNew.Visibility = Visibility.Hidden;
    }
    private async void btnSearch_Click(object sender, RoutedEventArgs e)
    {
        if (txtSearchJ.Text.Length != 14)
        {
            MessageBox.Show("JSHSHIRni to'liq kiriting iltimos.");
            return;
        }
        var existUser = await userService.RetrieveByJSHSHIRAsync(txtSearchJ.Text);
        if (existUser is not null)
        {
            userJshshir = existUser.JSHSHIR;
            MessageBox.Show($"{txtSearchJ.Text} JSHSHIR li mijoz mavjud.");
            btnExistClient.Visibility = Visibility.Visible;
        }
        else
        {
            MessageBox.Show($"{txtSearchJ.Text} JSHSHIR li mijoz mavjud emas.");
            btnNewAdd.Visibility = Visibility.Visible;
        }
    }
    private void btnExistClient_Click(object sender, RoutedEventArgs e)
    {
        if (rbtnYurdik.IsChecked.Equals(true))
        {
            spYurCutomer.Visibility = Visibility.Visible;
            spJisCutomer.Visibility = Visibility.Hidden;
            spYattCutomer.Visibility = Visibility.Hidden;
            spYurYattJis.Visibility = Visibility.Visible;
            spQaytish.Visibility = Visibility.Hidden;
            spJisNew.Visibility = Visibility.Hidden;
        }
        else if (rbtnYaTT.IsChecked.Equals(true))
        {
            spYurCutomer.Visibility = Visibility.Hidden;
            spJisCutomer.Visibility = Visibility.Hidden;
            spYattCutomer.Visibility = Visibility.Visible;
            spYurYattJis.Visibility = Visibility.Visible;
            spQaytish.Visibility = Visibility.Hidden;
            spJisNew.Visibility = Visibility.Hidden;
        }
    }
    private void btnYaTTRahbar_Click(object sender, RoutedEventArgs e)
    {

        spYurYattJis.Visibility = Visibility.Hidden;
        spYurCutomer.Visibility = Visibility.Hidden;
        spJisCutomer.Visibility = Visibility.Hidden;
        spYattCutomer.Visibility = Visibility.Hidden;
        spQaytish.Visibility = Visibility.Visible;
        spJisNew.Visibility = Visibility.Visible;
    }
    private async void btnYattAdd_Click(object sender, RoutedEventArgs e)
    {
        CustomerCreationDto customerCreationDto = new CustomerCreationDto();

        customerCreationDto.Name = txtYattNomi.Text.ToLower();
        if (txtYattJSHSHIR.Text.Length != 14)
        {
            MessageBox.Show("JSHSHIR ni to'liq kiriting iltimos.");
            return;
        }
        customerCreationDto.JSHSHIR = txtYattJSHSHIR.Text;
        customerCreationDto.MFO = txtYattMFO.Text;

        if (txtYattXisobRaqam.Text.Length != 23)
        {
            MessageBox.Show("Xisob raqamni to'liq kiriting iltimos.");
            return;
        }
        customerCreationDto.BankAccount = RemoveDashes(txtYattXisobRaqam.Text);
        
        customerCreationDto.BankName = txtYattBank.Text.ToLower();
        customerCreationDto.YurAddress = txtYattFirmaAdres.Text.ToLower();
        customerCreationDto.Phone = txtYattTelefon.Text;

        if (userJshshir.Equals(""))
        {
            MessageBox.Show("YaTT rahbarini ma'lumotni kiriting.");
            return;
        }
        var existUser = await this.userService.RetrieveByJSHSHIRAsync(userJshshir);
        customerCreationDto.UserId = existUser.Id;

        if (customerCreationDto.Name.Equals("") ||
            customerCreationDto.Phone.Equals("") ||
            customerCreationDto.MFO.Equals("") ||
            customerCreationDto.BankName.Equals("") ||
            customerCreationDto.YurAddress.Equals(""))
            MessageBox.Show("Ma'lumotni to'liq kiriting!");
        else
        {
            var result = this.customerService.AddAsync(customerCreationDto);

            if (!result.IsCompletedSuccessfully)
            {
                MessageBox.Show($" Saqlandi.");
                IsCreated = true;
            }
            else
                MessageBox.Show($"{"Saqlashda xatolik"}");
        }
    }

    // qo'shimcha funsiya
    // xisob raqamni - larni olib tashlaydigan function
    public string RemoveDashes(string input)
        => new string(input.Where(char.IsDigit).ToArray());
   
        //TextBoxlar
    private async void txtJisJSHSHIR_TextChanged(object sender, TextChangedEventArgs e)
    {
        
        ValidationHelper.ValidateOnlyNumberInput(sender as TextBox);

        if (txtJisJSHSHIR.Text.Length.Equals(14))
        {
            var existUser = await this.userService.RetrieveByJSHSHIRAsync(txtJisJSHSHIR.Text);
            if (existUser is not null)
            {
                txtFamiliya.Text = existUser.LastName.ToUpper();
                txtIsmi.Text = existUser.FirstName.ToUpper();
                txtSharifi.Text = existUser.Patronomyc.ToUpper();
                txtPasportSeria.Text = existUser.SeriaPasport;
                txtJisAdres.Text =existUser.Address.ToUpper();
                txtJisTelefon.Text = existUser.Phone;
                txtJisTelegramRaqam.Text = existUser.TelegramPhone;

                btnJisAdd.Visibility = Visibility.Hidden;

                MessageBox.Show($"JSHSHIR = {txtJisJSHSHIR.Text} li Jismoniy shaxs mavjud. Qayta ro'yxatdan o'tish shart emas.");
            }
        }
    }
    private void txtJisTelefon_TextChanged(object sender, TextChangedEventArgs e)
    {
        ValidationHelper.ValidatePhone(sender as TextBox);
    }
    private void txtJisTelegramRaqam_TextChanged(object sender, TextChangedEventArgs e)
    {
        ValidationHelper.ValidatePhone(sender as TextBox);
    }
    private void txtPasportSeria_TextChanged(object sender, TextChangedEventArgs e)
    {
        ValidationHelper.ValidatePasportInformation(sender as TextBox);
    }
    private async void txtYurINN_TextChanged(object sender, TextChangedEventArgs e)
    {
        ValidationHelper.ValidateOnlyNumberInput(sender as TextBox);

        if (txtYurINN.Text.Length == 9)
        {
            var inn = int.Parse(txtYurINN.Text);
            var existCustomer = await this.customerService.RetrieveByInnAsync(inn);
            if (existCustomer is not null)
            {
                txtYurNomi.Text = existCustomer.Name.ToUpper();
                txtYurPhone.Text = existCustomer.Phone;
                txtYurMFO.Text = existCustomer.MFO;
                txtYurXisobRaqam.Text = existCustomer.BankAccount;
                txtYurBank.Text = existCustomer.BankName.ToUpper();
                txtYurOKONX.Text = existCustomer.OKONX;
                txtYurFirmaAdres.Text = existCustomer.YurAddress.ToUpper();

                btnYurAdd.Visibility = Visibility.Hidden;
                MessageBox.Show($"INN:{txtYurINN.Text} mijoz mavjud. Qayta ro'yxatdan o'tkazish shart emas. ");
            }
        }
    }
    private void txtYurMFO_TextChanged(object sender, TextChangedEventArgs e)
    {
        ValidationHelper.ValidateOnlyNumberInput(sender as TextBox);
    }
    private void txtYurOKONX_TextChanged(object sender, TextChangedEventArgs e)
    {
        ValidationHelper.ValidateOnlyNumberInput(sender as TextBox);
    }
    private void txtYurXisobRaqam_TextChanged(object sender, TextChangedEventArgs e)
    {
        ValidationHelper.ValidateAccountNumber(sender as TextBox);
    }
    private async void txtYattJSHSHIR_TextChanged(object sender, TextChangedEventArgs e)
    {
        ValidationHelper.ValidateOnlyNumberInput(sender as TextBox);

        if (txtYattJSHSHIR.Text.Length.Equals(14))
        {
            var existYaTT = await this.customerService.RetrieveByJshshirAsync(txtYattJSHSHIR.Text);
            if (existYaTT is not null)
            {
                txtYattNomi.Text = existYaTT.Name.ToUpper();
                txtYattMFO.Text = existYaTT.MFO;
                txtYattXisobRaqam.Text = existYaTT.BankAccount;
                txtYattBank.Text = existYaTT.BankName.ToUpper();
                txtYattFirmaAdres.Text = existYaTT.YurAddress.ToUpper();
                txtYattTelefon.Text = existYaTT.Phone;
                btnYattAdd.Visibility = Visibility.Hidden;

                MessageBox.Show($"JSHSHIR = {txtYattJSHSHIR.Text} li YaTT mavjud. Qayta ro'yxatdan o'tish shart emas.");
            }
        }
    }
    private void txtYurPhone_TextChanged(object sender, TextChangedEventArgs e)
    {
        ValidationHelper.ValidatePhone(sender as TextBox);
    }
    private void txtYattXisobRaqam_TextChanged(object sender, TextChangedEventArgs e)
    {
        ValidationHelper.ValidateAccountNumber(sender as TextBox);
    }
    private void txtYattMFO_TextChanged(object sender, TextChangedEventArgs e)
    {
        ValidationHelper.ValidateOnlyNumberInput(sender as TextBox);
    }
    private void txtYattTelefon_TextChanged(object sender, TextChangedEventArgs e)
    {
        ValidationHelper.ValidatePhone(sender as TextBox);
    }
    private void txtEmpPasportSeria_TextChanged(object sender, TextChangedEventArgs e)
    {
        ValidationHelper.ValidatePasportInformation(sender as TextBox);
    }
    private void txtEmployeeJSHSHIR_TextChanged(object sender, TextChangedEventArgs e)
    {
        ValidationHelper.ValidateOnlyNumberInput(sender as TextBox);
    }
    private void txtEmpPhone_TextChanged(object sender, TextChangedEventArgs e)
    {
        ValidationHelper.ValidatePhone(sender as TextBox);
    }
    private void txtEmpTelegram_TextChanged(object sender, TextChangedEventArgs e)
    {
        ValidationHelper.ValidatePhone(sender as TextBox);
    }

    private void btnEmpAdd_Click(object sender, RoutedEventArgs e)
    {

    }

    private void rbtnAgent_Checked(object sender, RoutedEventArgs e)
    {

    }

    private void rbtnOmborchi_Checked(object sender, RoutedEventArgs e)
    {

    }

    private void rbtnSotuvchi_Checked(object sender, RoutedEventArgs e)
    {

    }

    private void rbtnBoshqaruvchi_Checked(object sender, RoutedEventArgs e)
    {

    }

    private void RadioButton1_Checked(object sender, RoutedEventArgs e)
    {
        if (sender is RadioButton radioButton)
        {
            // Tanlangan radio tugmani yashil rangga bo'yash
            radioButton.Background = (Brush)TryFindResource("SecondaryBackground");

            // Boshqa radio tugmalar fonini asl holida qoldirish
            foreach (var rb in new[] { rbtnAgent, rbtnOmborchi, rbtnSotuvchi, rbtnBoshqaruvchi })
            {
                if (rb != radioButton)
                {
                    rb.Background = (Brush)FindResource("SelectedBackground");
                }
            }
        }
    }

    private void btnSaveEmployee_Click(object sender, RoutedEventArgs e)
    {

    }
}

