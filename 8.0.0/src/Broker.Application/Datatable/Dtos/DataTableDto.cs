using Abp.Application.Services.Dto;
using Broker.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Datatable.Dtos
{
    public class DataTableInputDto 
    {
        public Column[] columns { get; set; }
        public Order[] order { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public int draw { get; set; }
        public bool? isDeleted { get; set; }
        public string creatorUserName { get; set; }
        public DateTime? creationTimeFrom { get; set; }
        public DateTime? creationTimeTo { get; set; }
        public string modifierUserName { get; set; }
        public DateTime? lastModificationTimeFrom { get; set; }
        public DateTime? lastModificationTimeTo { get; set; }
        public string deleterUserName { get; set; }
        public DateTime? deletionTimeFrom { get; set; }
        public DateTime? deletionTimeTo { get; set; }
        public string actionType { get; set; }
        public string action { get; set; }
        public string[] ids { get; set; }
        public string lang { get; set; }
    }

    public class DataTableOutputDto<T>
    {
        public decimal? stockValue { get; set; }
        public int draw { get; set; }
        public string actionResult { get; set; }
        public int iTotalRecords { get; set; }
        public int iTotalDisplayRecords { get; set; }
        public IReadOnlyList<T> aaData { get; set; }
        public DataTableOutputDto()
        {
        }
        public DataTableOutputDto(int _iTotalRecords, int _iTotalDisplayRecords, IReadOnlyList<T> _items)
        {
            iTotalRecords = _iTotalRecords;
            iTotalDisplayRecords = _iTotalDisplayRecords;
            aaData = _items;
        }
    }

    public class Column
    {
        public string name { get; set; }
        public bool orderable { get; set; }
        public bool searchable { get; set; }
        public Search search { get; set; }
    }

    public class Order
    {
        public int column { get; set; }
        public string dir { get; set; }
    }

    public class Search
    {
        public bool regex { get; set; }
        public string value { get; set; }
    }
}
