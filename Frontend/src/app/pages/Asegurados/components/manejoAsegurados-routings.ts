import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ManejoAseguradosComponent } from './manejo-asegurados/manejo-asegurados.component';

export const routes: Routes = [
  {path:'asegurados',
component: ManejoAseguradosComponent

}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: []
})
export class ManejoAseguradosRoutingModule { }
