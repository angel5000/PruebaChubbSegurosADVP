import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastrModule } from 'ngx-toastr';
import { ReplacePointWithCommaPipe } from '../../../pipes/replace-point-with-comma.pipe';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../../shared/shared.module';
import { ListTableComponent } from '../../../shared/components/Reusables/list-table/list-table.component';
import { ManejoAseguradosComponent } from './manejo-asegurados/manejo-asegurados.component';
import { DialogComponent } from './dialog/dialog-asegurado/dialog.component';
import { SubirArchivosComponent } from '../../../shared/components/Reusables/subir-archivos/subir-archivos.component';
import { DialogSegurosclientesComponent } from './dialog/dialog-seguros/dialog-seguros.component';
import { BuscadorComponent } from '../../../shared/components/Reusables/buscador/buscador.component';

@NgModule({
  declarations: [
    ManejoAseguradosComponent,
    DialogComponent,
    DialogSegurosclientesComponent
  ],
  exports:[],
  imports: [
    CommonModule,
    SharedModule,
    ListTableComponent,
    ToastrModule,
    FormsModule,
    ReplacePointWithCommaPipe,
    SubirArchivosComponent,
    BuscadorComponent
]
})
export class AseguradosModule { }
