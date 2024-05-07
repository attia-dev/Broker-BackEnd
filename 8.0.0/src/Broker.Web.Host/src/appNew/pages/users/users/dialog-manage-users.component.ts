import { Component, Injector, OnInit } from '@angular/core';
import { NbDialogRef } from '@nebular/theme';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import { UserDto, UserServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
    selector: 'ngx-dialog-manage-users',
    templateUrl: 'dialog-manage-users.component.html',
    styleUrls: ['users.component.scss'],
})
export class DialogManageUsersComponent extends AppComponentBase implements OnInit {

    userId: number;
    selectedObj: UserDto;
    public userForm: FormGroup;

    constructor(injector: Injector, private fb: FormBuilder,private _userService: UserServiceProxy, protected ref: NbDialogRef<DialogManageUsersComponent>) {
        super(injector);
        this.selectedObj = new UserDto();
        this.userForm = this.fb.group({
            name: [this.selectedObj.name, [Validators.required]],
            surName: [this.selectedObj.surname, [Validators.required]],
           // fullName: [this.selectedObj.fullName, [Validators.required]],
            emailAddress: [this.selectedObj.emailAddress, [Validators.required, Validators.pattern("^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$")]],
            userName: [this.selectedObj.userName, [Validators.pattern("^[a-z0-9_-]{1,15}$")]],
            phoneNumber: [this.selectedObj.phoneNumber, [Validators.pattern("^[a-z0-9_-]{3,15}$")]],
            password: [''],// pattern="(?=^.{8,}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\s)[0-9a-zA-Z!@#$%^&*()]*$"
            isActive: [this.selectedObj.isActive, Boolean],
        })
    }

    ngOnInit() {
        console.log("UserId", this.selectedObj.id);
        if (this.selectedObj.id > 0)
            this.getUser(this.selectedObj.id);
    }
    getUser(id) {
        this._userService.get(id)
            .pipe( 
                finalize(() => {

                })
        ).subscribe((result: UserDto) => {
            this.selectedObj = result;
            this.userForm = this.fb.group({
                name: [this.selectedObj.name, [Validators.required]],
                surName: [this.selectedObj.surname, [Validators.required]],
                //f: [this.selectedObj.fullName, [Validators.required]],
                emailAddress: [this.selectedObj.emailAddress, [Validators.required, Validators.pattern("^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$")]],
                userName: [this.selectedObj.userName, [Validators.pattern("^[a-z0-9_-]{1,15}$")]],
                phoneNumber: [this.selectedObj.phoneNumber, [Validators.pattern("^[a-z0-9_-]{3,15}$")]],
                password: [''],// pattern="(?=^.{8,}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\s)[0-9a-zA-Z!@#$%^&*()]*$"
                isActive: [this.selectedObj.isActive, Boolean],
            })
        
        });
       
    }
    cancel() {
        this.ref.close();
    }

    submit() {
        
        console.log(this.selectedObj)
        this.selectedObj.roleNames = [];
        if (this.selectedObj.id > 0)
            this.update(this.selectedObj);
        else
            this.create(this.selectedObj);
    }

    update(input) {
        debugger;
        this._userService.update(input)
            .pipe(
                finalize(() => {
                })
            )
            .subscribe(() => {
                debugger;
                this.notify.info(this.l('Common.Message.ActionSuccess'));
                setTimeout(() => {
                    this.ref.close(true);
                }, 1000);
            });
    }
    create(input) {
        debugger;
        this._userService.create(input)
            .pipe(
                finalize(() => {
                })
            )
            .subscribe(() => {
                debugger;
                this.notify.info(this.l('Common.Message.ActionSuccess'));
                setTimeout(() => {
                    this.ref.close(true);
                }, 1000);
            });
    }

}
