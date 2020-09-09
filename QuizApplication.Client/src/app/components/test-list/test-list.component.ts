import { Component, OnInit } from '@angular/core';
import { TestDetailed } from 'src/app/models/test-detailed';
import { TestService } from 'src/app/services/test.service';

@Component({
  selector: 'app-test-list',
  templateUrl: './test-list.component.html',
  styleUrls: ['./test-list.component.css']
})
export class TestListComponent implements OnInit {

  loading = true;
  tests: TestDetailed[];


  constructor(public testService: TestService,
              ) { }

  ngOnInit() {
    this.loadTests();
  }

  private loadTests() {
    this.loading = true;
    this.testService.getTests().subscribe((data: TestDetailed[]) => {
      this.tests = data;
      this.loading = false;
    }, error => {
        console.log(error);
    });   
  }
}
