


  function getTableColumns() {
    return [

        {
            label: "ID",
            property: "idusrseguros",
            type: "number",
            csslabel: ["font-bold", "text-sm"],
            cssProperty: ["font-semibold", "text-sm", "text-left"],
            sticky: false,
            sort: true,
            visible: true,
            download: true,
          },
      {
        label: "Nombre del Seguro",
        property: "nmbrseguro",
        type: "text",
        csslabel: ["font-bold", "text-sm"],
        cssProperty: ["font-semibold", "text-sm", "text-left"],
        sticky: false,
        sort: true,
        visible: true,
        download: true,
      },
       
      {
        label: "Cod. del seguro",
        property: "codseguro",
        type: "text",
        csslabel: ["font-bold"],
        cssProperty: ["font-semibold",  "text-left"],
        sticky: false,
        sort: true,
        visible: true,
        download: true,
      },
       
      {
        label: "Suma del seguro",
        property: "sumasegurada",
        type: "currency",
        csslabel: ["font-bold", "text-sm"],
        cssProperty: ["font-semibold", "text-sm", "text-left"],
        sticky: false,
        sort: true,
        visible: true,
        download: true,
      },
       
      {
        label: "Prima",
        property: "prima",
        type: "currency",
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
        type: "date",
        csslabel: ["font-bold", "text-sm"],
        cssProperty: ["font-semibold", "text-sm", "text-left"],
        sticky: false,
        sort: true,
        visible: true,
        download: true,
      },
    
      {
        label:"",
        csslabel:[],
        cssProperty:[''],
        property: "icDelete",
        cssSubProperty:["bi bi-trash3 fs-5"],
        type:"icon",
    action:"remove",
    tooltip:"eliminar",
    sticky:false,
    sort:false,
    visible:true,
    download:false
    }
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
  