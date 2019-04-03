/**
Jquery easyui datagrid js����excel
�޸���extgrid����excel
* allows for downloading of grid data (store) directly into excel
* Method: extracts data of gridPanel store, uses columnModel to construct XML excel document,
* converts to Base64, then loads everything into a data URL link.
*
* @author Animal <extjs support team>
*
*/
$.extend($.fn.datagrid.methods, {
    getExcelXml: function (jq, param) {
        var worksheet = this.createWorksheet(jq, param);
        //alert($(jq).datagrid('getColumnFields'));
        var totalWidth = 0;
        var cfs = $(jq).datagrid('getColumnFields');
        for (var i = 1; i < cfs.length; i++) {
            totalWidth += $(jq).datagrid('getColumnOption', cfs[i]).width;
        }
        
        //var totalWidth = this.getColumnModel().getTotalWidth(includeHidden);
        return '<?xml version="1.0" encoding="utf-8"?>\n' +//xml���������⣬��������ע����utf-8���룬�����gb2312����Ҫ�޸Ķ�̬ҳ�ļ���д�����
    '<ss:Workbook xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns:o="urn:schemas-microsoft-com:office:office">' +
    '<o:DocumentProperties><o:Title>' + param.title + '</o:Title></o:DocumentProperties>\n' +
    '<ss:ExcelWorkbook>' +
    '<ss:WindowHeight>' + worksheet.height + '</ss:WindowHeight>' +
    '<ss:WindowWidth>' + worksheet.width + '</ss:WindowWidth>' +
    '<ss:ProtectStructure>False</ss:ProtectStructure>' +
    '<ss:ProtectWindows>False</ss:ProtectWindows>' +
    '</ss:ExcelWorkbook>' +
    '<ss:Styles>' +
    '<ss:Style ss:ID="Default">' +
    '<ss:Alignment ss:Vertical="Top"  />\n' +
    '<ss:Font ss:FontName="arial" ss:Size="10" />\n' +
    '<ss:Borders>' +
    '<ss:Border  ss:Weight="1" ss:LineStyle="Continuous" ss:Position="Top" />\n' +
    '<ss:Border  ss:Weight="1" ss:LineStyle="Continuous" ss:Position="Bottom" />\n' +
    '<ss:Border  ss:Weight="1" ss:LineStyle="Continuous" ss:Position="Left" />\n' +
    '<ss:Border ss:Weight="1" ss:LineStyle="Continuous" ss:Position="Right" />\n' +
    '</ss:Borders>' +
    '<ss:Interior />\n' +
    '<ss:NumberFormat />\n' +
    '<ss:Protection />\n' +
    '</ss:Style>' +
    '<ss:Style ss:ID="title">' +
    '<ss:Borders />\n' +
    '<ss:Font />\n' +
    '<ss:Alignment  ss:Vertical="Center" ss:Horizontal="Center" />\n' +
    '<ss:NumberFormat ss:Format="@" />\n' +
    '</ss:Style>' +
    '<ss:Style ss:ID="headercell">' +
    '<ss:Font ss:Bold="1" ss:Size="10" />\n' +
    '<ss:Alignment  ss:Horizontal="Center" />\n' +
    '<ss:Interior ss:Pattern="Solid"  />\n' +
    '</ss:Style>' +
    '<ss:Style ss:ID="even">' +
    '<ss:Interior ss:Pattern="Solid"  />\n' +
    '</ss:Style>' +
    '<ss:Style ss:Parent="even" ss:ID="evendate">' +
    '<ss:NumberFormat ss:Format="yyyy-mm-dd" />\n' +
    '</ss:Style>' +
    '<ss:Style ss:Parent="even" ss:ID="evenint">' +
    '<ss:NumberFormat ss:Format="0" />\n' +
    '</ss:Style>' +
    '<ss:Style ss:Parent="even" ss:ID="evenfloat">' +
    '<ss:NumberFormat ss:Format="0.00" />\n' +
    '</ss:Style>' +
    '<ss:Style ss:ID="odd">' +
    '<ss:Interior ss:Pattern="Solid"  />\n' +
    '</ss:Style>' +
    '<ss:Style ss:Parent="odd" ss:ID="odddate">' +
    '<ss:NumberFormat ss:Format="yyyy-mm-dd" />\n' +
    '</ss:Style>' +
    '<ss:Style ss:Parent="odd" ss:ID="oddint">' +
    '<ss:NumberFormat ss:Format="0" />\n' +
    '</ss:Style>' +
    '<ss:Style ss:Parent="odd" ss:ID="oddfloat">' +
    '<ss:NumberFormat ss:Format="0.00" />\n' +
    '</ss:Style>' +
    '</ss:Styles>' +
    worksheet.xml +
    '</ss:Workbook>';
    },
   
    createWorksheet: function (jq, param) {
        
        // Calculate cell data types and extra class names which affect formatting
        var cellType = [];
        var cellTypeClass = [];
        //var cm = this.getColumnModel();
        var totalWidthInPixels = 0;
        var colXml = '';
        var headerXml = '';
        var visibleColumnCountReduction = 0;
        //��ȡ��������Ϣ
        //var dgOptions = $(jq).datagrid('getColumnFields');
        var cfsColumn = $(jq).datagrid('getColumnFields',false);
        var cfsFrozen = $(jq).datagrid('getColumnFields',true);
        var cfs = cfsFrozen.concat( cfsColumn );
        var colCount = cfs.length;
        for (var i = 0; i < colCount; i++) {        		
            if (cfs[i] != '') {
                var columnsOptions = $(jq).datagrid('getColumnOption', cfs[i]);
                if (columnsOptions.doNotExport!=undefined && columnsOptions.doNotExport)
                {
                		continue;
                }
                var w = columnsOptions.width;
                totalWidthInPixels += w;
                if (cfs[i] === "") {
                    cellType.push("None");
                    cellTypeClass.push("");
                    ++visibleColumnCountReduction;
                }
                else {
                		if (w!=undefined)
                    	colXml += '<ss:Column ss:AutoFitWidth="1" ss:Width="'+w+'" />\n\n';
                    else
                    	colXml += '<ss:Column ss:AutoFitWidth="1" ss:Width="130" />\n\n';
                    headerXml += '\n\t<ss:Cell ss:StyleID="headercell">' +
                '<ss:Data ss:Type="String">' + $(jq).datagrid('getColumnOption', cfs[i]).title + '</ss:Data>' +
                '<ss:NamedCell ss:Name="Print_Titles" />\n\t</ss:Cell>';
                    cellType.push("String");
                    cellTypeClass.push("");
                }
            }
        }
        var visibleColumnCount = cellType.length - visibleColumnCountReduction;
        var result = {
            height: 700,
            width: Math.floor(totalWidthInPixels * 30) + 50
        };
        //var rows = $(jq).datagrid('getRows');
        param.page = "1";
        param.rows = $('#cmpDG').datagrid('getData').total;
        var rows = "";
        $.ajax({
            url: 'JsonDataList',
            data: param ,
            async: false,//true,
            dataType: "json",
            success: function (datas) {
                rows = datas.rows;  //��ȡdatagird��������
            }
        });
        // Generate worksheet header details.
        var t = '<ss:Worksheet ss:Name="' + param.title + '">' +
		    '<ss:Names>' +
		    '<ss:NamedRange ss:Name="Print_Titles" ss:RefersTo="=\'' + param.title + '\'!R1:R2" />\n' +
		    '</ss:Names>' +
		    '<ss:Table x:FullRows="1" x:FullColumns="1"' +
		    ' ss:ExpandedColumnCount="' + (visibleColumnCount + 2) +
		    '" ss:ExpandedRowCount="' + (rows.length + 2) + '">' +
		    colXml +
		    '<ss:Row ss:AutoFitHeight="1">' +
		    headerXml +
		    '\n</ss:Row>\n';
		    // Generate the data rows from the data in the Store
		    for (var i = 0, it = rows, l = it.length; i < l; i++) {
		        t += '<ss:Row>\n';
		        var cellClass = (i & 1) ? 'odd' : 'even';
		        r = it[i];
		        var k = 0;		        		        
		        for (var j = 0; j < colCount; j++) {
		            if (cfs[j] != '') {
		                var v = r[cfs[j]];
		                var columnsOptions = $(jq).datagrid('getColumnOption', cfs[j]);
		                if (columnsOptions.doNotExport!=undefined && columnsOptions.doNotExport)
		                {
		                		continue;
		                }
		                if (cellType[k] !== "None") {
		                    t += '\n\t<ss:Cell ss:StyleID="' + cellClass + cellTypeClass[k] + '"><ss:Data ss:Type="' + cellType[k] + '">';
		                    if (v==null)
		                    	t += "";
		                    else if (columnsOptions.exportValue!=undefined && columnsOptions.exportValue!=null)
		                    	t += columnsOptions.exportValue(v);
		                    else if (cellType[k] == 'DateTime') {
		                        t += v.format('Y-m-d');
		                    } else {
		                        t += v;
		                    }
		                    t += '</ss:Data></ss:Cell>';
		                }
		                k++;
		            }
		        }
		        t += '\n</ss:Row>\n';
		    }
		    result.xml = t + '</ss:Table>' +
		    '<x:WorksheetOptions>' +
		    '<x:PageSetup>' +
		    '<x:Layout x:CenterHorizontal="1" x:Orientation="Landscape" />\n' +
		    '<x:Footer x:Data="Page &P of &N" x:Margin="0.5" />\n' +
		    '<x:PageMargins x:Top="0.5" x:Right="0.5" x:Left="0.5" x:Bottom="0.8" />\n' +
		    '</x:PageSetup>' +
		    '<x:FitToPage />\n' +
		    '<x:Print>' +
		    '<x:PrintErrors>Blank</x:PrintErrors>' +
		    '<x:FitWidth>1</x:FitWidth>' +
		    '<x:FitHeight>32767</x:FitHeight>' +
		    '<x:ValidPrinterInfo />\n' +
		    '<x:VerticalResolution>600</x:VerticalResolution>' +
		    '</x:Print>' +
		    '<x:Selected />\n' +
		    '<x:DoNotDisplayGridlines />\n' +
		    '<x:ProtectObjects>False</x:ProtectObjects>' +
		    '<x:ProtectScenarios>False</x:ProtectScenarios>' +
		    '</x:WorksheetOptions>' +
		    '</ss:Worksheet>';
        return result;
    }
});