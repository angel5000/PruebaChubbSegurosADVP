import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastrModule } from 'ngx-toastr';
import { ReplacePointWithCommaPipe } from '../../pipes/replace-point-with-comma.pipe';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { ListTableComponent } from '../../shared/components/Reusables/list-table/list-table.component';
import { SubirArchivosComponent } from '../../shared/components/Reusables/subir-archivos/subir-archivos.component';
import { BuscadorComponent } from '../../shared/components/Reusables/buscador/buscador.component';
import { ManejoCobranzasComponent } from './components/manejo-cobranzas/manejo-cobranzas.component';

@NgModule({
  declarations: [
    ManejoCobranzasComponent
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
export class CobranzasModule { }
