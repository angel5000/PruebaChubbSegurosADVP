import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { ManejoSegurosComponent } from './manejo-seguros/manejo-seguros.component';



export const routes: Routes = [
  {path:'seguros',
component: ManejoSegurosComponent

}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: []
})
export class ManejoSegurosRoutingModule { }
