import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../../../shared/shared.module';
import { ListTableComponent } from '../../../../shared/components/Reusables/list-table/list-table.component';
import { ToastrModule } from 'ngx-toastr';
import { ReplacePointWithCommaPipe } from '../../../pipes/replace-point-with-comma.pipe';
import { FormsModule } from '@angular/forms';
import { BienvenidaComponent } from './bienvenida/bienvenida.component';


@NgModule({
  declarations: [
   BienvenidaComponent,

  ],
  exports:[],
  imports: [
    CommonModule,
    SharedModule,
    ListTableComponent,
    ToastrModule,
    FormsModule,
    ReplacePointWithCommaPipe
]
})
export class BienvenidaModule { }
