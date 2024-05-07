import { Component, Injector, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppComponentBase } from '@shared/app-component-base';
import {GetUserWithPermissionsOutput, SaveUserPermissionsInput, UserDto, UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
@Component({
    selector: 'app-permission-user',
    templateUrl: './permission-user.component.html',
    styleUrls: ['./permission-user.component.scss']
})
export class PermissionUserComponent extends AppComponentBase implements OnInit {

    groupedPermissions: any[] = [];
    permissionSource = {};
    user: UserDto;
    userDto: UserDto;
    public userForm: FormGroup;
    userId = "0";
    userID: any;
    check: boolean;
    checkNumber: boolean=false;
    showPermission: boolean=true;
    constructor(injector: Injector, private fb: FormBuilder, private _userService: UserServiceProxy, private route: ActivatedRoute, private router: Router) {
        super(injector);
        this.userID = this.route.snapshot.paramMap.get('id');
        this.groupedPermissions = [];
        this.getWithPermissionsById();
    }

    ngOnInit() {
    }
    getWithPermissionsById() {
        this.userId = this.route.snapshot.paramMap.get('id');
        this._userService.getWithPermissionsById(+this.userId)
            .subscribe((result: GetUserWithPermissionsOutput) => {
                console.log("GetUserWithPermissionsOutput", result);
                this.fillPermissions(result);
            });
    }

    fillPermissions(data) {
        this.groupedPermissions = [];
        this.permissionSource = {};
        this.user = data.user;
        this.user.name = data.user.name;
        console.log("permissionSource", this.permissionSource);
        for (const item of data.allPermissions) {
            this.fillGroupedPermissionsList(item, data, false, "All");
        }
        console.log("groupedPermissions", this.groupedPermissions);
    }

    fillGroupedPermissionsList = (item, data, isChild, parentSystemName) => {
        this.permissionSource[item.name] = {};
        this.permissionSource[item.name].name = item.name;
        this.permissionSource[item.name].isGranted = false;
        this.permissionSource[item.name].isGrantedUpdated = false;
        for (const userPermission of data.grantedPermissions) {
            if (userPermission.name === item.name) {
                this.permissionSource[item.name].isGranted = true;
                this.permissionSource[item.name].isGrantedUpdated = true;
                break;
            }
        }
        const duplicate = $.grep(this.groupedPermissions, function (e) { return e.parentName === item.systemName; });
        if (duplicate.length === 0) {
            const groupedItem = {
                isChild: isChild,
                parentSystemName: parentSystemName,
                parentName: item.systemName,
                localizedName: this.l(item.displayName),
                readName: 'Read.' + item.systemName,
                writeName: 'Write.' + item.systemName,
                deleteName: 'Delete.' + item.systemName
            };
            this.groupedPermissions.push(groupedItem);
        }
        if (item.children)
            for (const child of item.children) {
                this.fillGroupedPermissionsList(child, data, true, item.systemName);
            }
    }

    checkAll = (type) => {
        if (type == 'Read') {
            $('[parent="Read.All"]').each(function (e) {
                if ($(this).is(':checked') !== $('#readAll').is(':checked'))
                    $(this).click();
            });
        }
        else if (type == 'Write') {
            $('[parent="Write.All"]').each(function (e) {
                if ($(this).is(':checked') !== $('#writeAll').is(':checked'))
                    $(this).click();
            });
        }
        else if (type == 'Delete') {
            $('[parent="Delete.All"]').each(function (e) {
                if ($(this).is(':checked') !== $('#deleteAll').is(':checked'))
                    $(this).click();
            });
        }
    }

    checkRead = (permission, type) => {
        if (type == 'Read') {
            this.permissionSource[permission.readName].isGrantedUpdated = !this.permissionSource[permission.readName].isGrantedUpdated;
            $('[parent="' + permission.readName + '"]').each(function (e) {
                if ($(this).is(':checked') !== $('[id*="' + permission.readName + '"]').is(':checked'))
                    $(this).click();
            });
        }
        else if (type == 'Write') {
            this.permissionSource[permission.writeName].isGrantedUpdated = !this.permissionSource[permission.writeName].isGrantedUpdated;
            $('[parent="' + permission.writeName + '"]').each(function (e) {
                if ($(this).is(':checked') !== $('[id*="' + permission.writeName + '"]').is(':checked'))
                    $(this).click();
            });
        } 
        else if (type == 'Delete') {
            this.permissionSource[permission.deleteName].isGrantedUpdated = !this.permissionSource[permission.deleteName].isGrantedUpdated;
            $('[parent="' + permission.deleteName + '"]').each(function (e) {
                if ($(this).is(':checked') !== $('[id*="' + permission.deleteName + '"]').is(':checked'))
                    $(this).click();
            });
        }
        console.log(this.permissionSource);
        if (type !== 'Read' && !this.permissionSource[permission.readName].isGrantedUpdated) {
            this.permissionSource[permission.readName].isGrantedUpdated = true;
        }
        if (type === 'Read' && !this.permissionSource[permission.readName].isGrantedUpdated) {
            if (this.permissionSource[permission.writeName])
                this.permissionSource[permission.writeName].isGrantedUpdated = false;
            if (this.permissionSource[permission.deleteName])
                this.permissionSource[permission.deleteName].isGrantedUpdated = false;
        }
    }

    savePermissions = () => {
        debugger;
        const grantedPermissions = [];
        for (const permission of this.groupedPermissions) {
            if (this.permissionSource[permission.readName] && this.permissionSource[permission.readName].isGrantedUpdated)
                grantedPermissions.push(this.permissionSource[permission.readName].name);
            if (this.permissionSource[permission.writeName] && this.permissionSource[permission.writeName].isGrantedUpdated)
                grantedPermissions.push(this.permissionSource[permission.writeName].name);
            if (this.permissionSource[permission.deleteName] && this.permissionSource[permission.deleteName].isGrantedUpdated)
                grantedPermissions.push(this.permissionSource[permission.deleteName].name);
        }
        const input = new SaveUserPermissionsInput();
        input.userId = +this.userId;
        input.grantedPermissions = grantedPermissions;
        abp.ui.setBusy();
        this._userService.savePermissions(input)
            .pipe(
                finalize(() => {
                    abp.ui.clearBusy();
                    window.location.reload();
                }))
            .subscribe(() => {
                for (const permission of this.groupedPermissions) {
                    if (this.permissionSource[permission.readName])
                        this.permissionSource[permission.readName].isGranted = permission.isGrantedUpdated;
                    if (this.permissionSource[permission.writeName])
                        this.permissionSource[permission.writeName].isGranted = permission.isGrantedUpdated;
                    if (this.permissionSource[permission.deleteName])
                        this.permissionSource[permission.deleteName].isGranted = permission.isGrantedUpdated;
                }

                //notify user that permission changed
                //SignalRClient.notifyPermissionsChanged(null, $stateParams.id);
                //abp.notify.success(App.localize('Common.Message.ActionSuccess'));
            });
        this.router.navigate(['/admin/pages/users/users']);
    };
    BackList() {
        this.router.navigate(['/admin/pages/users/users']);
    }
}
