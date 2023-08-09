import { Component, OnInit, ViewChild } from '@angular/core';
import { StudentService } from '../student.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Student } from '../../models/api-models/student.model';
import { Gender } from '../../models/api-models/gender.model';
import { GenderService } from '../../services/gender.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-view-student',
  templateUrl: './view-student.component.html',
  styleUrls: ['./view-student.component.css']
})
export class ViewStudentComponent implements OnInit {
  studentId:string | null |undefined ;
  student: Student={
    id:'',
    firstName:'',
    lastName:'',
    dateOfBirth:'',
    email:'',
    mobile:0,
    genderId:'',
    profileImageUrl:'',
    gender:{
      id:'',
      description:''
    },
    address:{
      id:'',
      physicalAddress:'',
      postalAddress:''
    }


  }
  isNewStudent=false;
  header='';
  displayProfileImageUrl='';
  genderList:Gender[]=[];
  @ViewChild('studentDetailsForm') studentDetailsForm?: NgForm;

 constructor(private readonly StudentService:StudentService,private readonly route:ActivatedRoute,
  private readonly GenderService:GenderService,
  private snckabar:MatSnackBar,
  private router:Router){}

 ngOnInit(): void {
   this.route.paramMap.subscribe(
    (params)=>{
      this.studentId=  params.get('id');
      if(this.studentId){
        if(this.studentId.toLowerCase() === 'Add'.toLowerCase() ){
            this.isNewStudent=true;
          this.header='Add New Student';
          this.setImage();

        }
        else{
          this.isNewStudent=false;
          this.header='Edit Student';
          this.StudentService.getStudent(this.studentId).subscribe(
            (success)=>{
             this.student= success;
             this.setImage();
            },
            (error)=>{
            this.setImage();
            }
          );
        }



        this.GenderService.getGenderList().subscribe(
          (success)=>{
              this.genderList=success;
          }
        );

      }
    }
   );
 }

 onUpdate(): void{
  if(this.studentDetailsForm?.form.valid){
  this.StudentService.updateStudent(this.student.id,this.student)
  .subscribe(
    (success)=>{
        this.snckabar.open('Student Update Successfully',undefined,{
          duration:2000
        });

        setTimeout(() => {
          this.router.navigateByUrl('students');
        },2000);
    },
    (error)=>{
       console.log(error);
       
    }
  )
  }
 }

 onDelete():void{
  this.StudentService.deleteStudent(this.student.id).subscribe(
    (success)=>{
      this.snckabar.open('Student Deleted Successfully',undefined,{
        duration:2000
      });

      setTimeout(() => {
        this.router.navigateByUrl('students');
      },2000);
    },
    (error)=>{

    }
  )
 }
 

 onAdd():void{
   if(this.studentDetailsForm?.form.valid){

    this.StudentService.addStudent(this.student).subscribe(
      (success)=>{
        this.snckabar.open('Student added Successfully',undefined,{
          duration:2000
        });

        setTimeout(() => {
          this.router.navigateByUrl('students');
        },2000);
        
      },
      (error)=>{
        console.log(error);
      }
    );

   } 
 }

private setImage():void{
  if(this.student.profileImageUrl){
 this.displayProfileImageUrl= this.StudentService.getImagePath(this.student.profileImageUrl) ;
  }
  else{
    this.displayProfileImageUrl='/assets/user.jpg';
  }
 } 

 uploadImage(event:any):void{
if(this.studentId){
  const file:File=event.target.files[0];
  this.StudentService.uploadImage(this.student.id,file).subscribe(
    (success)=>{
      this.student.profileImageUrl=success;
      this.setImage;
      
      //notify
      this.snckabar.open('Image Update Successfully',undefined,{
        duration:2000
      });
  
      location.reload();


    },
    (error)=>{

    }
  )
}
 }


}
