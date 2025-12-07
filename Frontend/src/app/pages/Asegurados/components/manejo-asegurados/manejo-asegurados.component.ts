import { Component } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Aseguramiento } from '../../Model/asegurados.interface';
import { DialogComponent } from '../dialog/dialog-asegurado/dialog.component';
import { AseguradosService } from '../services/servicios.service';
import { ComponentSettings } from './list.config';
import Swal from 'sweetalert2';
import { DialogSegurosclientesComponent } from '../dialog/dialog-seguros/dialog-seguros.component';
import { FilterBox } from '../../../../shared/models/SearchOptions.interface';
import { actualizarPermiso } from './list.config';
import { SegurosService } from '../../../Seguros/Servicios/seguros.service';
@Component({
  selector: 'app-manejo-asegurados',
  templateUrl: './manejo-asegurados.component.html',
  styleUrl: './manejo-asegurados.component.scss'
})
export class ManejoAseguradosComponent {

  component: any;
  mode: string = 'register'
  pantalla: string;

  constructor(public _dialog: MatDialog,
    private toastr: ToastrService, public aseguradosserv: AseguradosService,
    public seguroserv: SegurosService) {
  }

  ngOnInit(): void {
    this.component = ComponentSettings
    this.pantalla = 'asegurados'
    this.formatGetInputs()
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

  openDialogRegister(ocultar?: boolean, data?: number, accion?: string, mode?: string) {
    const dialogConfig = new MatDialogConfig()
    dialogConfig.data = { data, ocultar, accion, mode };
    this._dialog.open(DialogComponent, {
      data: dialogConfig.data,
      disableClose: false,
      width: '500px',

    }).afterClosed().subscribe((res) => {
      if (res) {
        this.setGetInputsProviders(true)
      }

    })
  }

  openDialogSeguros(data: any) {
    const dialogConfig = new MatDialogConfig()
    dialogConfig.data = { data };
    this._dialog.open(DialogSegurosclientesComponent, {
      data: dialogConfig.data,
      disableClose: false,
      width: '600px',
    }).afterClosed().subscribe((res) => {
      this.setGetInputsProviders(true)
    })
  }

  search(data: FilterBox) {
    const searchValue = data.searchData.toLocaleLowerCase()?.trim();
    const searchField = data.searchValue || null; // nuevo: campo a filtrar
    ComponentSettings.filters.numFilter=searchField.toString()
  console.log(data) ;
  this.component.filters.numFilter=1
    if (!searchValue) {

      this.component.filters = {};
     // actualizarPermiso.prototype.PermisoConsultar(true);
    } else {
      //actualizarPermiso.prototype.PermisoConsultar(false);
      if (searchField) {
        // filtrar por campo específico
        this.component.filters.textFilter = { [searchField]: searchValue };
        this.component.filters.numFilter=1
      } else {
        // si no hay campo, se puede filtrar por todos los campos relevantes
        this.component.filters.textFilter = { searchAll: searchValue };
        this.component.filters.numFilter=1
      }
    }
  
    this.setGetInputsProviders(true);
  }
  

  rowClick(e: any) {
    let action = e.action
    switch (action) {
      case "edit": this.openDialogRegister(false, e.row, action, 'actualizar')
        break
      case "ver": this.openDialogSeguros(e);
        break
      case "agregar": this.openDialogRegister(false, e.row, action, 'actualizar')
        break
      case "remove":
        this.EliminarAsegurado(e, e.row)
        break
    }
    return false
  }

  EliminarAsegurado(e: any, seg: Aseguramiento) {
    if (seg.codseguro !== '0') {
      Swal.fire({
        title: `Hay seguros asociados a este cliente, debe desvincular primero sus seguros para poder eliminarlo `,
        text: "¿Desea proceder?",
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
          this.openDialogSeguros(e);
        }
      })

    } else {
      Swal.fire({
        title: `¿Esta seguro que desea eliminar al asegurado: ${seg.nmbrcompleto}? `,
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
          this.aseguradosserv.EliminarAsegurado(e.row.idasegurados).subscribe({
            next: (response) => {
              if (response.isSucces) {
                this.toastr.success(response.message, 'Éxito');
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
    }
  }


}
