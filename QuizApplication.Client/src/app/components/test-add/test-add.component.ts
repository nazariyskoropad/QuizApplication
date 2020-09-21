import { Component, OnInit } from '@angular/core';
import { TestDetailed } from 'src/app/models/test-detailed';
import { FormGroup, FormBuilder, FormArray, Validators } from '@angular/forms';
import { TestService } from 'src/app/services/test.service';
import { Router } from '@angular/router';
import { QuestionAnswer } from 'src/app/models/questionAnswer';
import { Question } from 'src/app/models/question';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';

@Component({
  selector: 'app-test-add',
  templateUrl: './test-add.component.html',
  styleUrls: ['./test-add.component.css']
})
export class TestAddComponent implements OnInit {

  loading = false;
  test: TestDetailed;
  addForm: FormGroup;
  submitted = false;

  constructor(public testService: TestService,
    private router: Router,
    private fb: FormBuilder) {
    }

  ngOnInit() {
    this.initForm();
  }

  private initForm() {

    this.addForm = this.fb.group({
      TestName: ['', [Validators.required, Validators.maxLength(200)]],
      TestDescription: ['', [Validators.required, Validators.maxLength(200)]],
      TestTimeLimit: ['', Validators.required],
      TestPoints: ['', Validators.required],
      TestStartsAt: ['', Validators.required],
      TestEndsAt: ['', Validators.required],
      TestRunsNumber: ['', Validators.required],
      Questions: this.fb.array([])
    });
  }

  get questionForms() {
    return this.addForm.get('Questions') as FormArray;
  }

  addQuestion() {
    const question = this.fb.group({

      QuestionTimeLimit: ['', Validators.required],
      QuestionPoints: ['', Validators.required],
      QuestionDescription: ['', [Validators.required, Validators.maxLength(700)]],
      QuestionAnswers: this.fb.array([])
    });

    this.questionForms.push(question);
  }

  addQA(question) {
    const qa = this.fb.group({
      QaDescription: ['', [Validators.required, Validators.maxLength(700)]],
      QaIsTrue: [false, Validators.required]
    });

    question.get("QuestionAnswers").push(qa);
  }

  deleteQuestion(i) {
    this.questionForms.removeAt(i);
  }

  deleteQA(question, i) {
    question.get("QuestionAnswers").removeAt(i);
  }


  get f() {
    return this.addForm.controls;
  }

  createNewTest(){
    this.submitted = true;

    if (this.addForm.invalid) {
      return;
    }
    
    if(!this.validatePoints())
    {
      alert('Sum of points for question is not equal to test points')
      return;
    }

    this.initializeTest();

    this.testService.addTest(this.test).subscribe((data: TestDetailed) => {
      alert('Tests added successfully!');

      this.router.navigate(['/admin/tests', data.id]);
    }, error => {
      console.log(error);
    })
  }

  validatePoints() {
    let questionPoints = 0;
    this.f.Questions.value.forEach(q => questionPoints += q.QuestionPoints)
    return this.f.TestPoints.value == questionPoints;
  }

  initializeTest() {
    this.test = new TestDetailed();

    this.test.name = this.f.TestName.value;
    this.test.description = this.f.TestDescription.value;
    this.test.points = this.f.TestPoints.value;
    this.test.timeLimit = this.f.TestTimeLimit.value;
    this.test.startsAt = this.f.TestStartsAt.value;
    this.test.endsAt = this.f.TestEndsAt.value;
    
    for (let question of this.f.Questions.value) {
      let newQuestion = new Question()
      newQuestion.description = question.QuestionDescription;
      newQuestion.points = question.QuestionPoints;
      newQuestion.timeLimit = question.QuestionTimeLimit;

      for(let qa of question.QuestionAnswers) {
        let newQa = new QuestionAnswer();
        newQa.isCorrect = qa.QaIsTrue;
        newQa.description = qa.QaDescription;

        newQuestion.questionAnswers.push(newQa);
      }

      this.test.questions.push(newQuestion);
    }
  }
}
