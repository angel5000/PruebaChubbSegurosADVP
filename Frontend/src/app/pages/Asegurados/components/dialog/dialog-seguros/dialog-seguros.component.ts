import { ChangeDetectorRef, Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Aseguramiento } from '../../../Model/asegurados.interface';
import { ComponentSettings } from './list.config';
import Swal from 'sweetalert2';
import { AseguradosService } from '../../services/servicios.service';
import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'app-dialog-seguros',
  templateUrl: './dialog-seguros.component.html',
  styleUrl: './dialog-seguros.component.scss'
})
export class DialogSegurosclientesComponent {
  form: FormGroup
  ocultar: boolean = false;
  component: any;
  detalleColumns: any[] = []
  detalleSeguros: any;
  segurosAgrupados: any[] = [];
  constructor(@Inject(MAT_DIALOG_DATA) public data, private _fb: FormBuilder,
    public _dialogRef: MatDialogRef<DialogSegurosclientesComponent>,
    public aseguradosserv: AseguradosService,
    private toastr: ToastrService
  ) {
    if (data) {
      this.segurosAgrupados = data.data.row.seguros.map(x => ({
        idusrseguros: x.idusrseguros,
        nmbrseguro: x.nmbrseguro,
        codseguro: x.codseguro,
        sumasegurada: x.sumasegurada,
        prima: x.prima,
        fechacontrataseguro: x.fechacontrataseguro
      }));
      let segurosdispo = this.segurosAgrupados.filter(s => s.idusrseguros !== 0);
      if (segurosdispo.length !== 0) {
        this.detalleSeguros = this.segurosAgrupados;
      }
    }

    this.ocultar = this.data.ocultar || false
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

  rowClick(e: any) {
    let action = e.action
    console.log(e);
    switch (action) {
      case "remove":
        this.EliminarAseguramiento(e.row.idusrseguros, e.row)
        break
    }
    return false
  }

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
  }

}
