import { Component, OnInit } from '@angular/core';
import { TestService } from 'src/app/services/test.service';
import { ActivatedRoute, Router } from '@angular/router';
import { TestDetailed } from 'src/app/models/test-detailed';

@Component({
  selector: 'app-test-info',
  templateUrl: './test-info.component.html',
  styleUrls: ['./test-info.component.css']
})
export class TestInfoComponent implements OnInit {

  testId: number;
  test: TestDetailed;
  loading = true;

  constructor(private testService:TestService,
              private route: ActivatedRoute,
              private router: Router) { }

  ngOnInit() {
    this.loadTest();
  }

  private loadTest() {
    this.testId = Number(this.route.snapshot.paramMap.get('id'));
    this.testService.getTest(this.testId).subscribe((data: TestDetailed) => {
      this.test = data;
      this.loading = false;
    },
    error => {
      console.log(error);
    })
  }

  public deleteTest(id: number) {
    this.testService.deleteTest(id)
      .subscribe((res) => {
        alert('Test was successfully deleted');             
        this.router.navigate(['/admin/tests']);
      });
  }

  public editTest(id: number) {
    this.router.navigate(['/admin/tests', id, 'edit']);
  }
}
