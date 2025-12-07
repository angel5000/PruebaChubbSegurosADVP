import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ManejoCobranzasComponent } from './components/manejo-cobranzas/manejo-cobranzas.component';

export const routes: Routes = [
  {path:'cobranzas',
component: ManejoCobranzasComponent

}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: []
})
export class CobranzasRoutingModule { }