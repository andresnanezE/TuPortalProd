(function (noticias, $, undefined)
{ 
    var dataSource = new kendo.data.DataSource(
    {
        type: "json",
        transport:
        {
            read:
            {
                url: urlTraerNoticias,
                type: "POST",
                data: 
                {
                    soloActivas: true
                }
            }
        },
        schema:
        {
            data: "data",
            total: "total",
            model:
            {
                fields:
                {
                    NOTICIAID: { type: "string" },
                    TITULO: { type: "string" },
                    TITULOQS: { type: "string" },
                    DESCRIPCION: { type: "string" },
                    FECHA: { type: "date" },
                    ACTIVO: { type: "boolean" },
                    IMAGEN: { type: "string" },
                }
            }
        },
        sort: { field: "FECHA", dir: "asc" },
        pageSize: 10,
        serverPaging: true,
        serverFiltering: false,
        serverSorting: true
    });

    $("#pager").kendoPager(
    {
        dataSource: dataSource
    });

    $("#listView").kendoListView(
    {
        dataSource: dataSource,
        template: kendo.template($("#noticias-template").html())
    });

}(window.noticias = window.noticias || {}, jQuery));