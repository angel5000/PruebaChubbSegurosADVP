import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastrModule } from 'ngx-toastr';
import { ReplacePointWithCommaPipe } from '../../../pipes/replace-point-with-comma.pipe';
import { FormsModule } from '@angular/forms';
import { ManejoSegurosComponent } from './manejo-seguros/manejo-seguros.component';
import { SharedModule } from '../../../shared/shared.module';
import { ListTableComponent } from '../../../shared/components/Reusables/list-table/list-table.component';
import { DialogSegurosComponent } from './manejo-seguros/dialog-seguros/dialog-seguros.component';
import { BuscadorComponent } from '../../../shared/components/Reusables/buscador/buscador.component';
import { SubirArchivosComponent } from '../../../shared/components/Reusables/subir-archivos/subir-archivos.component';
import { DialogAseguradosComponent } from './manejo-seguros/dialog-asegurados/dialog-asegurados.component';


@NgModule({
  declarations: [
    ManejoSegurosComponent,
    DialogSegurosComponent,
    DialogAseguradosComponent
  ],
  exports:[],
  imports: [
    CommonModule,
    SharedModule,
    ListTableComponent,
    ToastrModule,
    FormsModule,
    BuscadorComponent,
    ReplacePointWithCommaPipe,
    SubirArchivosComponent
]
})
export class SegurosModule { }
