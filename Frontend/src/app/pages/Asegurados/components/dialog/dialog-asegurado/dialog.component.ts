import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { AseguradosRequest, Aseguramiento } from '../../../Model/asegurados.interface';
import { AseguradosService } from '../../services/servicios.service';

@Component({
  selector: 'app-dialog',
  templateUrl: './dialog.component.html',
  styleUrl: './dialog.component.scss'
})
export class DialogComponent implements OnInit {
  form: FormGroup
  ocultar: boolean = false;
  ocultarseg: boolean = false;
  getseguros: any[] = [];
isLoading: any;
mostrar: boolean = false;

  constructor(@Inject(MAT_DIALOG_DATA) public data, private _fb: FormBuilder,
    public _dialogRef: MatDialogRef<DialogComponent>,
    private toastr: ToastrService, public aseguradosserv: AseguradosService
  ) {
    this.initForm();

    if (data.data) {
      this.Segurosporid(data.data.idasegurados)
      this.ConsultaSegurosDisponibles(data.data.edad)
    } if (data.accion === 'edit') {
      this.ocultarseg = true
    }
    this.ocultar = this.data.ocultar || false
  }

  ngOnInit(): void {
    this.form.get('edad')?.valueChanges.subscribe(value => {
      const edad = Number(value);
      if (!isNaN(edad) && edad > 0) {
        this.ConsultaSegurosDisponibles(edad);
      }
    });
    this.form.get('segdisponibles')?.valueChanges.subscribe(value => {
     console.log(value);
     
      if (value!==null) {
      this.mostrar=true
      }else{
        this.mostrar=false
      }
    });
    if (this.data.accion == 'edit') {
      this.form.get('cedula')?.disable();
    }
  }

  initForm(): void {
    this.form = this._fb.group({
      id: [''],
      cedula: ['', [Validators.required]],
      nmbrcompleto: [''],
      telefono: ['', [Validators.required]],
      edad: ['', [Validators.required]],
      segdisponibles: [''],
    });
  }

  onInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    let value = input.value.replace(/[^0-9]/g, '')
    input.value = value;
  }

  clearSeguro() {
    this.form.get('segdisponibles')?.setValue(null); // borra la selección
  }
  
  GuardarAsegurado() {
    this.isLoading = true
    if (this.data.accion == 'edit') {
      const form = this.form.getRawValue();
      const idseg = form.id;
      const request: AseguradosRequest = {
        cedula: form.cedula,
        nmbrcompleto: form.nmbrcompleto,
        telefono: form.telefono,
        edad: Number(form.edad),
        usrActualizacion:'ANGEL',
        estado:1
      };

      this.aseguradosserv.ActualizarAsegurado(idseg, request).subscribe({
        next: (response) => {
          if (response.isSucces) {
            this.toastr.success(response.message, 'Éxito');
            this.form.reset({}, { emitEvent: false });
            this.form.markAsPristine();
            this.form.markAsUntouched();
            this._dialogRef.close(response.isSucces);
            this.isLoading = false
          }
          else {
            this.isLoading = false
            this.toastr.warning(response.message, 'Advertencia');
          }
        },
        error: (err) => {
          this.isLoading = false
          this.toastr.error(err, 'Error');

        }
      });

    } else if (this.data.accion == 'agregar') {
      this.EjecutarRegistroAseguramiento();
    }
    else {
      const form = this.form.value;
      const request: AseguradosRequest = {
        cedula: form.cedula,
        nmbrcompleto: form.nmbrcompleto,
        telefono: form.telefono,
        edad: Number(form.edad)
      };

      this.aseguradosserv.RegistrarAsegurado(request).subscribe({
        next: (response) => {
          if (response.isSucces) {
            this.toastr.success(response.message, 'Éxito');
            console.log(form.segdisponibles);
            if (form.segdisponibles) {
              this.EjecutarRegistroAseguramiento()
            }
            this.form.reset({}, { emitEvent: false });
            this.form.markAsPristine();
            this.form.markAsUntouched();
            this.isLoading = false
            this._dialogRef.close(response.isSucces);
          }
          else {
            this.isLoading = false
            this.toastr.warning(response.message, 'Advertencia');
          }
        },
        error: (err) => {
          this.isLoading = false
          this.toastr.error(err, 'Error');

        }
      });
    }
  }

  EjecutarRegistroAseguramiento() {
    const form = this.form.value;
    const request: Aseguramiento = {
      cedula: form.cedula,
      codseguro: form.segdisponibles
    };
    this.aseguradosserv.RegistrarAseguramiento(request).subscribe({
      next: (response) => {
        if (response.isSucces) {
          this.toastr.success(response.message, 'Éxito');
          this.form.reset({}, { emitEvent: false });
          this.form.markAsPristine();
          this.form.markAsUntouched();
          this._dialogRef.close(response.isSucces);
          this.isLoading = false
        }
        else {
          this.isLoading = false
          this.toastr.warning(response.message, 'Advertencia');
        }
      },
      error: (err) => {
        this.isLoading = false
        this.toastr.error(err.message, 'Error');

      }
    });
  }

  ConsultaSegurosDisponibles(edad: number) {
    this.aseguradosserv.ObtenerSegdisponibles(edad).subscribe({
      next: (response) => {
        if (response.isSucces) {
          this.getseguros = response.data.map((x: any) => ({
            value: x.codseguro,
            label: x.nmbrseguro
          }));
        }
        else {
          this.toastr.warning(response.message, 'Advertencia');
        }
      },
      error: (err) => {
        this.toastr.error(err, 'Error');
      }
    });

  }

  Segurosporid(id: number) {
    this.aseguradosserv.AseguradoPorId(id).subscribe({
      next: (response) => {
        if (response.isSucces) {
          this.form.reset({
            id: response.data.idasegurados,
            cedula: response.data.cedula,
            nmbrcompleto: response.data.nmbrcompleto,
            telefono: response.data.telefono,
            edad: response.data.edad
          });
        } else {
          this.toastr.error(response.message, 'Error');
        }
      },
      error: (err) => {
        this.toastr.error(err, 'Error');

      }
    });
  }


}
