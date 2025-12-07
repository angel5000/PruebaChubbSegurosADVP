function getTableColumns() {
    return [

        {
            label: "ID",
            property: "idasegurados",
            type: "number",
            csslabel: ["font-bold", "text-sm"],
            cssProperty: ["font-semibold", "text-sm", "text-left"],
            sticky: false,
            sort: true,
            visible: true,
            download: true,
          },
      {
        label: "Cedula",
        property: "cedula",
        type: "text",
        csslabel: ["font-bold", "text-sm"],
        cssProperty: ["font-semibold", "text-sm", "text-left"],
        sticky: false,
        sort: true,
        visible: true,
        download: true,
      },
       
      {
        label: "Nombre Completo",
        property: "nmbrcompleto",
        type: "text",
        csslabel: ["font-bold"],
        cssProperty: ["font-semibold",  "text-left"],
        sticky: false,
        sort: true,
        visible: true,
        download: true,
      },
       
      {
        label: "Edad",
        property: "edad",
        type: "number",
        csslabel: ["font-bold", "text-sm"],
        cssProperty: ["font-semibold", "text-sm", "text-left"],
        sticky: false,
        sort: true,
        visible: true,
        download: true,
      },
       
      {
        label: "Telefono",
        property: "telefono",
        type: "text",
        csslabel: ["font-bold", "text-sm"],
        cssProperty: ["font-semibold", "text-sm", "text-left"],
        sticky: false,
        sort: true,
        visible: true,
        download: true,
      },
       
      {
        label: "Fecha de Contrato",
        property: "fechacontrataseguro",
        type: "text",
        csslabel: ["font-bold", "text-sm"],
        cssProperty: ["font-semibold", "text-sm", "text-left"],
        sticky: false,
        sort: true,
        visible: true,
        download: true,
      },
    
    
    ];
  }




const filters={
    numFilter:"",
    textFilter:"",
    stateFilter: null,
    startDate: null,
    endDate: null,
    refresh:false
}

const resetFilters={
    numFilter:"",
    textFilter:"",
    stateFilter: null,
    startDate: null,
    endDate: null,
    refresh: false
};
const Permisos={
eliminar:false,
editar:false,
consultar:false
};
export class actualizarPermiso {
  PermisoEliminar(valor: boolean) {
    Permisos.eliminar = valor;
  
    // Actualizar las columnas dinámicamente
    ComponentSettings.tableColumns = getTableColumns();
  }
  PermisoEditar(valor: boolean) {
    Permisos.editar = valor;
  
    // Actualizar las columnas dinámicamente
    ComponentSettings.tableColumns = getTableColumns();
  }
}


const getInputs: string="";
export const ComponentSettings={
   Permisos,
    tableColumns:getTableColumns(),
    getInputs,
    resetFilters,
    filters:filters,
    filters_dates_active:false,
 
   
    }
  