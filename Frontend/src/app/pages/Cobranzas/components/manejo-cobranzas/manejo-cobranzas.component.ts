import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { CobrazansService } from '../Services/cobrazansservices.service';
import Swal from 'sweetalert2';
@Component({
  selector: 'app-manejo-cobranzas',
  templateUrl: './manejo-cobranzas.component.html',
  styleUrl: './manejo-cobranzas.component.scss'
})
export class ManejoCobranzasComponent implements OnInit{
  listaCobranzas: any[] = [];
  usuario: string;
  constructor(public _dialog: MatDialog,
    private toastr: ToastrService,
    public cobraserv: CobrazansService) {
  }
  ngOnInit(): void {
 this.ConsultarCobranzas();
 this.usuario = (localStorage.getItem('usuario') ?? '').replace(/"/g, '');
  }
  ConsultarCobranzas() {

   
    this.cobraserv.Obtenerdatos().subscribe({
      next: (response) => {
        if (response.isSucces) {
       this.listaCobranzas = response.data;
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
  esMora(estado: string): boolean {
    return estado === 'MORA' || estado === 'VENCIDO';
  }
  confirmarCancelacion(item: any) {
    console.log(item);
    
    Swal.fire({
      title: '¿Cancelar Póliza?',
      text: `Se desactivará el seguro y se anularán los cobros pendientes. Los pagos ya realizados se mantendrán en el historial.`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      confirmButtonText: 'Sí, cancelar póliza'
    }).then((result) => {
      if (result.isConfirmed) {

        this.cobraserv.CancelarSeguro(item.idasegurado,this.usuario).subscribe(resp => {
          if(resp.isSucces){
            this.toastr.success(resp.message);
            this.cargarDatos(); 
          }else{
            this.toastr.error(resp.message);
   
          }
        
        });
      }
    });
  }
  cargarDatos() {
    this.ConsultarCobranzas();
  }
  abrirModalCobrar(item: any){

  }
  enviarWhatsapp(item: any){

  }
}
