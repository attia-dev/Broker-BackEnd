using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.MultiTenancy;
using Broker.Authorization.Users;
using Broker.Datatable.Dtos;

namespace Broker.Users.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserDto : EntityDto<long>
    {
        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        public bool IsActive { get; set; }

        public string FullName { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public DateTime CreationTime { get; set; }

        public string[] RoleNames { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public bool IsWhatsApped { get; set; }
    }

    public class GetUsersInput : DataTableInputDto
    {
        public string Name { get; set; }
    }
    public class SaveUserPermissionsInput
    {
        public long userId { get; set; }
        public int? TenantId { get; set; }
        public string[] grantedPermissions { get; set; }
    }
    public class GetUserWithPermissionsOutput
    {
        public UserDto User { get; set; }
        public List<PermissionCustomDto> GrantedPermissions { get; set; }
        public List<PermissionCustomDto> AllPermissions { get; set; }
    }

    public class PermissionCustomDto
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string SystemName { get; set; }
        public string Description { get; set; }
        public MultiTenancySides MultiTenancySides { get; set; }
        public List<PermissionCustomDto> Children { get; set; }
    }
}
