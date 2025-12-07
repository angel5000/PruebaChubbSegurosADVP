import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Asegurados } from '../../../../Asegurados/Model/asegurados.interface';
import { SegurosService } from '../../../Servicios/seguros.service';
import { ComponentSettings } from './list.config';

@Component({
  selector: 'app-dialog-asegurados',
  templateUrl: './dialog-asegurados.component.html',
  styleUrl: './dialog-asegurados.component.scss'
})
export class DialogAseguradosComponent {
  form: FormGroup
  ocultar: boolean = false;
  component: any;
  detalleColumns: any[] = []
  detalleSeguros: any;
  segurosAgrupados: any[] = [];
  constructor(@Inject(MAT_DIALOG_DATA) public data, private _fb: FormBuilder,
    public _dialogRef: MatDialogRef<DialogAseguradosComponent>,
    public aseguradosserv: SegurosService,
    private toastr: ToastrService
  ) {
    if (data) {
      
    }

    this.ocultar = this.data.ocultar || false
  }

  ngOnInit(): void {
    this.component = ComponentSettings
    this.AseguradosPorseguros(this.data.id)
  }

  formatGetInputs() {
    let str = "";
    if (this.component.filters.refresh) {
      const random = Math.random();
      str += `&refresh=${random}`;
      this.component.filters.refresh = false;
    }
    this.component.getInputs = str;
  }

  setGetInputsProviders(refresh: boolean) {
    this.component.filters.refresh = refresh;
    this.formatGetInputs()
  }

  rowClick(e: any) {
    let action = e.action
    console.log(e);
    switch (action) {
      case "remove":
        //this.EliminarAseguramiento(e.row.idusrseguros, e.row)
        break
    }
    return false
  }
  AseguradosPorseguros(id: number) {
    this.aseguradosserv.AseguradosPorseguro(id).subscribe({
      next: (response) => {
        console.log(id,response);
        
        if (response) {
          const request: Asegurados = {
            idasegurados: response.data.idasegurados,
            cedula: response.data.cedula,
            nmbrcompleto: response.data.nmbrcompleto,
            edad: response.data.edad,
            telefono: response.data.telefono,
            fechacontrataseguro: response.data.fechacontrataseguro

          }
          this.detalleSeguros=[request];
        }
      },
      error: (err) => {
        this.toastr.error(err, 'Error');
      }
    });
  }
  /*
  EliminarAseguramiento(id: number, seg: Aseguramiento) {

    Swal.fire({
      title: `¿Esta seguro que desea eliminar este seguro: ${seg.nmbrseguro}? `,
      text: "Se borrara permanentemente",
      icon: "warning",
      showCancelButton: true,
      focusCancel: true,
      confirmButtonColor: 'rgb(210,155,253)',
      cancelButtonColor: 'rgb(79,109,253)',
      cancelButtonText: 'cancelar',
      confirmButtonText: 'OK',
      width: 430

    }).then((result) => {
      if (result.isConfirmed) {
        this.aseguradosserv.EliminarAseguramiento(id).subscribe({
          next: (response) => {
            if (response.isSucces) {
              this.toastr.success(response.message, 'Éxito');
              const index = this.segurosAgrupados.findIndex(
                seguro => seguro.idusrseguros === id
              );
              // Eliminar de la lista tambien
              if (index !== -1) {
                this.segurosAgrupados.splice(index, 1);
                this.detalleSeguros = [...this.segurosAgrupados];
              }
              this.setGetInputsProviders(true)
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
    })
  }*/

}
