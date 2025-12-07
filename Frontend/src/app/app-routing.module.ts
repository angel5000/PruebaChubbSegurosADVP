import { NgModule } from '@angular/core';
import {  RouterModule, Routes } from '@angular/router';
import { ManejoAseguradosComponent } from './pages/Asegurados/components/manejo-asegurados/manejo-asegurados.component';
import { LoginComponent } from './pages/Auth/components/login/login.component';
import { BienvenidaComponent } from './pages/Bienvenida/Principal/components/bienvenida/bienvenida.component';
import { ManejoSegurosComponent } from './pages/Seguros/components/manejo-seguros/manejo-seguros.component';


 const routes: Routes = [
  {
    path: '',
    redirectTo: 'bienvenido', 
    pathMatch: 'full', 
   
  },
    {
      path: 'bienvenido',
      component: BienvenidaComponent,
      loadChildren: () => import('./pages/Bienvenida/Principal/components/bienvenida-modules').then((m) => m.BienvenidaModule)
    },
    {
    
      path: 'seguros',
    component:ManejoSegurosComponent,
      loadChildren: () => import('./pages/Seguros/components/manejoSeguros-modules').then((m) => m.SegurosModule)
    },
    {
    
      path: 'asegurados',
    component:ManejoAseguradosComponent,
      loadChildren: () => import('./pages/Asegurados/components/manejoAsegurados-modules').then((m) => m.AseguradosModule)
    },
    {
    
      path: 'login',
    component:LoginComponent,
      loadChildren: () => import('./pages/Auth/Auth-modules').then((m) => m.AuthModule)
    },
    { path: '**', component: BienvenidaComponent },
   
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
  })
  
  export class AppRoutingModule {
  }