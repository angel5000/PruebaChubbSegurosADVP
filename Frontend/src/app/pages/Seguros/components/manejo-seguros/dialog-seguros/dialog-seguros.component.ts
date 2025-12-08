import { getLocaleDateFormat } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { SegurosRequest } from '../../../Models/seguros.interface';
import { SegurosService } from '../../../Servicios/seguros.service';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { Estados } from '../../../../../../static-data/config';
import { AuthService } from '../../../../Auth/Services/auth.service';
@Component({
  selector: 'app-dialog-seguros',
  templateUrl: './dialog-seguros.component.html',
  styleUrl: './dialog-seguros.component.scss'
})
export class DialogSegurosComponent implements OnInit {

  form: FormGroup
  ocultar: boolean = false;
  isLoading: boolean = false;
  getestado :any;
  usuario:string=""
  constructor(@Inject(MAT_DIALOG_DATA) public data, private _fb: FormBuilder,
    public _dialogRef: MatDialogRef<DialogSegurosComponent>,
    private toastr: ToastrService, private seguroserv: SegurosService,private http: HttpClient) {
    this.initForm();
    if (data.id) {
      this.Segurosporid(data.id)
    }
    this.ocultar = this.data.ocultar || false
  }

  ngOnInit(): void {
    if (this.data.accion == 'edit') {
      this.form.get('codseguro')?.disable();
    }
    this.getestado=Estados
    this.usuario = (localStorage.getItem('usuario') ?? '').replace(/"/g, '');
  }

  initForm(): void {
    this.form = this._fb.group({
      id: [''],
      nmbrseguro: ['', [Validators.required]],
      codseguro: ['',[Validators.required]],
      sumasegurada: ['', [Validators.required]],
      prima: ['', [Validators.required]],
      edadmin: ['', [Validators.required]],
      edadmax: ['', [Validators.required]],
      estado: ['', [Validators.required]]
    }, {
      validators: this.validarRangoEdad
    });
  }

  validarRangoEdad(form: FormGroup) {
    const min = Number(form.get('edadmin')?.value);
    const max = Number(form.get('edadmax')?.value);
  
    if (min !== null && max !== null && max < min) {
      return { rangoInvalido: true };
    }
  
    return null;
  }

  onInputnumbsimple(event: Event): void {
  const input = event.target as HTMLInputElement;
  let value = input.value.replace(/[^0-9]/g, '')
  input.value = value;
  }

  onInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    let value = input.value
      .replace(/[^0-9.]/g, '')      // solo numeros y punto
      .replace(/(\.)(?=.*\.)/g, ''); // solo un punto permitido
    if (value.includes('.')) {//2 decimales
      const [intPart, decPart] = value.split('.');
      value = intPart + '.' + decPart.substring(0, 2);
    }
    input.value = value;
  }

  async GuardarSeguro() {
    this.isLoading = true;
    const ipuser = await this.getClientIp();
    if (this.data.accion == 'edit') {
      const form = this.form.getRawValue();
      const idseg = form.id;
      const request: SegurosRequest = {
        nmbrseguro: form.nmbrseguro,
        codseguro: form.codseguro,
        sumasegurada: Number(form.sumasegurada),
        prima: Number(form.prima),
        edadmin: Number(form.edadmin),
        edadmax: Number(form.edadmax),
        usrActualizacion: this.usuario ,
        estado:form.estado,
        usuarioIP:ipuser
      }; 
     this.seguroserv.ActualizarSeguro(idseg, request).subscribe({
        next: (response) => {
          if (response.isSucces) {
            this.toastr.success(response.message, 'Éxito');
            this.isLoading = false
            this._dialogRef.close();
          }
          else {
            this.isLoading = false
            this.toastr.warning(response.message, 'Advertencia');
          }
        },
        error: (err) => {
          this.isLoading = false
          this.toastr.error(err.message);

        }
      });
    }

    else {
      const form = this.form.value;
      const request: SegurosRequest = {
        nmbrseguro: form.nmbrseguro,
        codseguro: form.codseguro,
        sumasegurada: Number(form.sumasegurada),
        prima: Number(form.prima),
        edadmin: Number(form.edadmin),
        edadmax: Number(form.edadmax),
        usrCreacion: this.usuario,
        estado:form.estado,
        usuarioIP:ipuser
      };
      this.seguroserv.RegistrarSeguro(request).subscribe({
        next: (response) => {
          if (response.isSucces) {
            this.toastr.success(response.message, 'Éxito');
            this.form.reset({}, { emitEvent: false });
            this.form.markAsPristine();
            this.form.markAsUntouched();
            Object.values(this.form.controls).forEach(control => {
              control.markAsPristine();
              control.markAsUntouched();
            });
            this.isLoading = false
            this._dialogRef.close();
          }
          else {
            this.isLoading = false
            this.toastr.warning(response.message, 'Advertencia');
          }
        },
        error: (err) => {
          this.isLoading = false
          this.toastr.error(err.message);
        }
      });
    }

  }

async getClientIp(): Promise<string> {
  try {

    const data = await firstValueFrom(
      this.http.get<{ ip: string }>('https://api.ipify.org?format=json')
    );
    return data.ip;
  } catch (error) {
    console.warn('No se pudo obtener la IP, usando default');
    return '0.0.0.0'; 
  }
}
  
  
  Segurosporid(id: number) {
    this.seguroserv.SeguroPorId(id).subscribe({
      next: (response) => {
        if (response) {
          const estadoNum = response.data.estado ? 1 : 0;

          console.log(estadoNum,response);
          
          this.form.reset({
            id: response.data.idseguro,
            nmbrseguro: response.data.nmbrseguro,
            codseguro: response.data.codseguro,
            sumasegurada: response.data.sumasegurada,
            prima: response.data.prima,
            edadmin: response.data.edadmin,
            edadmax: response.data.edadmax,
            estado:response.data.estado ? 1 : 0
          });
        }
      },
      error: (err) => {
        this.toastr.error(err, 'Error');
      }
    });
  }

}
