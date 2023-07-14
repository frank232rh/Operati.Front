import { Component, Inject, ViewChild } from '@angular/core';
import { FormControl, Validators, FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, FormGroupDirective } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { NgIf } from '@angular/common';
import { MatDialog, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { User } from '../interfaces/userInterfaz';
import { Response } from '../interfaces/responseInterfaz';
import { configInterfaz } from '../interfaces/configInterfaz';
import { alertaBasicaComponent } from '../alerts/alertaBasica.component';
import { from } from 'rxjs';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})

export class HomeComponent{

  //#region Properties
  public users: User[] = [];
  public user = {} as User;
  public response = { success: true, message: "" } as Response;
  displayedColumns: string[] = ['id', 'name', 'mail', 'actions'];
  isnew = true;
  hide = true;
  dataSource = new MatTableDataSource<User>(this.users);
  config = {} as configInterfaz
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(FormGroupDirective) formDirective: FormGroupDirective;
  //#endregion Properties

  //#region Validator
  dataForm = new FormGroup({
    name : new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required]),
    newPassword: new FormControl('', !this.isnew ? [Validators.required] : [Validators.nullValidator]),
    mail : new FormControl('', [Validators.email, Validators.required])
  });
  //#endregion Validator

  //#region Constructor
  constructor(
    public http: HttpClient,
    @Inject('BASE_URL') public baseUrl: string,
    private formBuilder: FormBuilder,
    public dialog: MatDialog
  ) {
    this.get();
  }
  //#endregion Constructor

  //#region Methods
  clearForm() {
    this.isnew = true;
    this.dataForm.controls.name.setValue('');
    this.dataForm.controls.name.setErrors(null);
    this.dataForm.controls.mail.setValue('');
    this.dataForm.controls.mail.setErrors(null);
    this.dataForm.controls.password.setValue('');
    this.dataForm.controls.password.setErrors(null);
    this.dataForm.controls.newPassword.setValue('');
    this.dataForm.controls.newPassword.setErrors(null);
  }

  get() {
    this.http.get<User[]>(this.baseUrl + 'user/GetUsers').subscribe(result => {
      this.dataSource = new MatTableDataSource<User>(result);
      this.dataSource.paginator = this.paginator;
    }, error => console.error(error));
  }

  getErrorMessage() {
    if (this.dataForm.controls.mail.hasError('required')) {
      return 'You must enter a value';
    }

    return this.dataForm.controls.mail.hasError('email') ? 'Not a valid email' : '';
  }

  modify(currentUser: User) {
    this.isnew = false;
    this.user = currentUser;
    this.dataForm.controls.name.setValue(currentUser.name);
    this.dataForm.controls.mail.setValue(currentUser.mail);

  }

  mostrarAlerta() {
    
    let dialogAlerta = this.dialog.open(alertaBasicaComponent,
      {
        width: '350px',
        disableClose: true,
        data: this.config
      });
  }

  save() {
    let u = {
      Id: 0,
      Name: this.dataForm.controls.name.value,
      Password: this.dataForm.controls.password.value,
      NewPassword : "",
      Mail: this.dataForm.controls.mail.value
    }

    const body = JSON.stringify(u);
    const headers = { 'content-type': 'application/json' };
    this.http.post<Response>(this.baseUrl + 'user/CreateUser', body, { 'headers': headers }).subscribe(data => {
      this.response = data;
      this.config = { tittle: data.success ? "Success" : "Fail", message: data.message };
      this.mostrarAlerta();
      if (data.success) {
        this.get();
        this.clearForm();
      }
    })

  }

  update() {
    let u = {
      Id: this.user.id,
      Name: this.dataForm.controls.name.value,
      Password: this.dataForm.controls.password.value,
      NewPassword: this.dataForm.controls.newPassword.value,
      Mail: this.dataForm.controls.mail.value
    }

    const body = JSON.stringify(u);
    const headers = { 'content-type': 'application/json' };
    this.http.post<Response>(this.baseUrl + 'user/ModifyUser', body, { 'headers': headers }).subscribe(data => {
      this.response = data;
      this.config = { tittle: data.success ? "Success" : "Fail", message: data.message };
      this.mostrarAlerta();
      if (data.success) {
        this.get();
        this.clearForm();
      }
    })

  }

  //#endregion Methods
}

