import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { RowClick } from '../../../../shared/models/RowClick.interface';
import { SegurosResponse } from '../../Models/seguros.interface';
import { SegurosService } from '../../Servicios/seguros.service';
import { DialogSegurosComponent } from './dialog-seguros/dialog-seguros.component';
import { ComponentSettings } from './list.config';
import Swal from 'sweetalert2';
import { ToastrService } from 'ngx-toastr';
import { FilterBox } from '../../../../shared/models/SearchOptions.interface';
@Component({
  selector: 'app-manejo-seguros',
  templateUrl: './manejo-seguros.component.html',
  styleUrl: './manejo-seguros.component.scss'
})
export class ManejoSegurosComponent implements OnInit {

  component: any;
  mode: string = 'register'
  constructor(
    public _dialog: MatDialog,
    public seguroserv: SegurosService,
    private toastr: ToastrService) {
  }

  ngOnInit(): void {
    this.component = ComponentSettings
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

  search(data: FilterBox) {
    const codigo = data.searchData?.trim();
    if (!codigo) {
      this.component.filters = {};
    } else {
      this.component.filters = { codseguro: codigo.toLowerCase() };
    }
    this.setGetInputsProviders(true);
  }

  openDialogRegister(ocultar?: boolean, id?: number, accion?: string, mode?: string) {
    const dialogConfig = new MatDialogConfig()
    dialogConfig.data = { id, ocultar, accion, mode };
    this._dialog.open(DialogSegurosComponent, {
      data: dialogConfig.data,
      disableClose: false,
      width: '500px',

    }).afterClosed().subscribe((res) => {
      this.setGetInputsProviders(true)
    })
  }

  rowClick(e: RowClick<SegurosResponse>) {
    let action = e.action
    switch (action) {
      case "edit": this.openDialogRegister(false, e.row.idseguro, action, 'actualizar')
        break
      case "remove":
        this.EliminarSeguro(e.row.idseguro, e.row)
        break
    }
    return false
  }


  EliminarSeguro(id: number, seg: SegurosResponse) {

    Swal.fire({
      title: `¿Esta seguro que desea eliminar el seguro: ${seg.nmbrseguro}? `,
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
        this.seguroserv.EliminarSeguro(id).subscribe({
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
