import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Chart, registerables } from 'chart.js';

Chart.register(...registerables); // Required for Chart.js v3+

@Component({
  selector: 'app-statistics',
  standalone: true,
  templateUrl: './statistics.component.html',
  styleUrls: ['./statistics.component.scss']
})
export class StatisticsComponent implements OnInit {
  numOfCustomers = 0;
  totalPolicies = 0;
  activePolicies = 0;

  policyTypeChart: Chart | undefined;
  premiumFrequencyChart: Chart | undefined;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.fetchStatistics();
  }

  fetchStatistics(): void {
    this.http.get<any>('https://localhost:7268/api/AdminStatistics')
      .subscribe({
        next: (data) => {
          this.numOfCustomers = data.userCountExcludingAdmins;
          this.totalPolicies = data.totalPolicies;
          this.activePolicies = data.activeUserPolicies;

          // Transform arrays to object maps for easier charting
          const policyDist = this.transformArrayToMap(data.policyTypeDistribution, 'policyType');
          const freqDist = this.transformArrayToMap(data.premiumFrequencyDistribution, 'premiumFrequency');

          this.renderPolicyTypeChart(policyDist);
          this.renderPremiumFrequencyChart(freqDist);
        },
        error: (err) => {
          console.error('Error fetching statistics:', err);
        }
      });
  }

  transformArrayToMap(array: any[], labelKey: string): { [key: string]: number } {
    const result: { [key: string]: number } = {};
    array.forEach(item => {
      result[item[labelKey]] = item.count;
    });
    return result;
  }

  renderPolicyTypeChart(distribution: { [key: string]: number }): void {
    if (this.policyTypeChart) this.policyTypeChart.destroy();

    const labels = Object.keys(distribution);
    const data = Object.values(distribution);

    this.policyTypeChart = new Chart('policyTypeChart', {
      type: 'bar',
      data: {
        labels,
        datasets: [{
          label: 'Policies',
          data,
          backgroundColor: '#4e79a7',
          borderRadius: 0,
          barPercentage: 0.7,
          categoryPercentage: 0.6

        }]
      },
      options: {
        responsive: true,
        plugins: {
          legend: { display: false },
          tooltip: {
            backgroundColor: '#333',
            titleColor: '#fff',
            bodyColor: '#fff',
            borderWidth: 1,
            borderColor: '#555',
            cornerRadius: 6,
            padding: 10,
            callbacks: {
              label: (context) => `${context.parsed.y} policies`
            }
          }
        },
        scales: {
          x: {
            ticks: {
              color: '#333',
              font: { size: 14 }
            },
            title: {
              display: true,
              text: 'Policy Type',
              color: '#333',
              font: { weight: 'bold', size: 14 }
            }
          },
          y: {
            beginAtZero: true,
            ticks: {
              stepSize: 1,
              color: '#333',
              font: { size: 14 }
            },
            title: {
              display: true,
              text: 'Count',
              color: '#333',
              font: { weight: 'bold', size: 14 }
            }
          }
        }
      }
    });
  }

  renderPremiumFrequencyChart(distribution: { [key: string]: number }): void {
    if (this.premiumFrequencyChart) this.premiumFrequencyChart.destroy();

    const labels = Object.keys(distribution);
    const data = Object.values(distribution);

    this.premiumFrequencyChart = new Chart('premiumFrequencyChart', {
      type: 'bar',
      data: {
        labels,
        datasets: [{
          label: 'Frequency',
          data,
          backgroundColor: '#59a14f',
          borderRadius: 0,
          barPercentage: 0.5,
          categoryPercentage: 0.6

        }]
      },
      options: {
        responsive: true,
        plugins: {
          legend: { display: false },
          tooltip: {
            backgroundColor: '#333',
            titleColor: '#fff',
            bodyColor: '#fff',
            borderWidth: 1,
            borderColor: '#555',
            cornerRadius: 6,
            padding: 10,
            callbacks: {
              label: (context) => `${context.parsed.y} entries`
            }
          }
        },
        scales: {
          x: {
            ticks: {
              color: '#333',
              font: { size: 14 }
            },
            title: {
              display: true,
              text: 'Frequency Type',
              color: '#333',
              font: { weight: 'bold', size: 14 }
            }
          },
          y: {
            beginAtZero: true,
            ticks: {
              stepSize: 1,
              color: '#333',
              font: { size: 14 }
            },
            title: {
              display: true,
              text: 'Count',
              color: '#333',
              font: { weight: 'bold', size: 14 }
            }
          }
        }
      }
    });
  }
}
