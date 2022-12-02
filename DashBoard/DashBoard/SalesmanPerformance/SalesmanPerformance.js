
        (function ($) {
            $(window).on("load", function () {
                $("#mscroll").DataTable({
                    "paging": false,
                    "searching": false,
                    "info": false,
                    "scrollY": "340px",
                    "scrollCollapse": true,
                });
            });


        })(jQuery);
$(document).ready(function () {

    // table data
    var tb1 = '';
    tb1 += '<tr class="selected"><td><img src="~/assests/images/bt3.jpg" class="img-circle mw90" /></td><td><span class="nme">Susanta Roy</span></td><td><span class="label label-success">1</span></td><td>90</td></tr>';
    tb1 += '<tr><td><img src="~/assests/images/bt5.jpg" class="img-circle mw90" /></td><td><span class="nme">Arun Mahato</span></td><td><span class="label label-info">2</span></td><td>82</td></tr>';
    tb1 += '<tr><td><img src="~/assests/images/bt2.jpg" class="img-circle mw90" /></td><td><span class="nme">Rajbir Sharma</span></td><td><span class="label label-warning">3</span></td><td>76</td></tr>';
    tb1 += '<tr><td><img src="~/assests/images/bt1.jpg" class="img-circle mw90" /></td><td><span class="nme">Bijay Thakur</span></td><td><span class="label label-primary">4</span></td><td>68</td></tr>';
    tb1 += '<tr><td><img src="~/assests/images/bt9.jpg" class="img-circle mw90" /></td><td><span class="nme">Ruby Sil</span></td><td>5</td><td>68</td></tr>';
    tb1 += '<tr><td><img src="~/assests/images/bt6.jpg" class="img-circle mw90" /> </td><td><span class="nme">Goutam Nandi</span></td><td>6</td><td>65</td></tr>';
    tb1 += '<tr><td><img src="~/assests/images/bt3.jpg" class="img-circle mw90" /></td><td><span class="nme">R Mohan</span></td><td>7</td><td>63</td></tr>';
    tb1 += '<tr><td><img src="~/assests/images/bt7.jpg" class="img-circle mw90" /></td><td><span class="nme">K Benugopal</span></td><td>8</td><td>57</td></tr>';
    //
    var tb1 = '';
    tb1 += '<tr class="selected"><td><img src="~/assests/images/bt1.jpg" class="img-circle mw90" /></td><td><span class="nme">Mohit Roy</span></td><td><span class="label label-success">1</span></td><td>90</td></tr>';
    tb1 += '<tr><td><img src="~/assests/images/bt2.jpg" class="img-circle mw90" /></td><td><span class="nme">Gurucharan Mahato</span></td><td><span class="label label-info">2</span></td><td>82</td></tr>';
    tb1 += '<tr><td><img src="~/assests/images/bt3.jpg" class="img-circle mw90" /></td><td><span class="nme">Anand Sharma</span></td><td><span class="label label-warning">3</span></td><td>76</td></tr>';
    tb1 += '<tr><td><img src="~/assests/images/bt4.jpg" class="img-circle mw90" /></td><td><span class="nme">Rohit Thakur</span></td><td><span class="label label-primary">4</span></td><td>68</td></tr>';
    tb1 += '<tr><td><img src="~/assests/images/bt8.jpg" class="img-circle mw90" /></td><td><span class="nme">Ruby Sil</span></td><td>5</td><td>68</td></tr>';
    tb1 += '<tr><td><img src="~/assests/images/bt5.jpg" class="img-circle mw90" /> </td><td><span class="nme">Goutam Nandi</span></td><td>6</td><td>65</td></tr>';
    tb1 += '<tr><td><img src="~/assests/images/bt6.jpg" class="img-circle mw90" /></td><td><span class="nme">R Mohan</span></td><td>7</td><td>63</td></tr>';
    tb1 += '<tr><td><img src="~/assests/images/bt7.jpg" class="img-circle mw90" /></td><td><span class="nme">K Benugopal</span></td><td>8</td><td>57</td></tr>';

















    var js1 = '<div class="statsHd sl1">24.4.2019</div><ul class="statsList cc1"><li><span class="blockage">Savings Max Account</span> <span class="blckNum">35</span></li><li><span class="blockage">Senior Citizen Account</span><span class="blckNum">40</span></li><li><span class="blockage">Two Wheeler Loan</span><span class="blckNum">15</span></li></ul><div class="statsHd sl2">23.4.2019</div><ul class="statsList cc2"><li><span class="blockage">Credit Cards</span> <span class="blckNum">26</span></li><li><span class="blockage">Health & Accident Insurance</span><span class="blckNum">35</span></li><li><span class="blockage">Personal Loan</span><span class="blckNum">19</span></li></ul>';

    var js2 = '<div class="statsHd sl1">24.4.2019</div><ul class="statsList cc1"><li><span class="blockage">Savings Max Account</span> <span class="blckNum">38</span></li><li><span class="blockage">Senior Citizen Account</span><span class="blckNum">12</span></li><li><span class="blockage">Two Wheeler Loan</span><span class="blckNum">32</span></li></ul><div class="statsHd sl2">23.4.2019</div><ul class="statsList cc2"><li><span class="blockage">Credit Cards</span> <span class="blckNum">26</span></li><li><span class="blockage">Health & Accident Insurance</span><span class="blckNum">35</span></li><li><span class="blockage">Personal Loan</span><span class="blckNum">19</span></li></ul>';

    var js3 = '<div class="statsHd sl1">24.4.2019</div><ul class="statsList cc1"><li><span class="blockage">Savings Max Account</span> <span class="blckNum">35</span></li><li><span class="blockage">Senior Citizen Account</span><span class="blckNum">30</span></li><li><span class="blockage">Two Wheeler Loan</span><span class="blckNum">11</span></li></ul><div class="statsHd sl2">23.4.2019</div><ul class="statsList cc2"><li><span class="blockage">Credit Cards</span> <span class="blckNum">26</span></li><li><span class="blockage">Health & Accident Insurance</span><span class="blckNum">35</span></li><li><span class="blockage">Personal Loan</span><span class="blckNum">19</span></li></ul>';

    $('.leaderTbl > tbody > tr').click(function () {

        var rowCount = $(this).index() + 1;

        $('.leaderTbl>tbody>tr').removeClass('selected');
        $(this).addClass('selected');

        if (rowCount == 1) {
            $('.statsWrap').html('');
            $('.statsWrap').html(js1);
        } else if (rowCount == 2) {
            $('.statsWrap').html('');
            $('.statsWrap').html(js2);
        } else if (rowCount == 3) {
            $('.statsWrap').html('');
            $('.statsWrap').html(js3);
        } else if (rowCount == 4) {
            $('.statsWrap').html('');
            $('.statsWrap').html(js2);
        } else if (rowCount == 5) {
            $('.statsWrap').html('');
            $('.statsWrap').html(js1);
        } else if (rowCount == 6) {
            $('.statsWrap').html('');
            $('.statsWrap').html(js3);
        } else if (rowCount == 7) {
            $('.statsWrap').html('');
            $('.statsWrap').html(js1);
        } else if (rowCount == 8) {
            $('.statsWrap').html('');
            $('.statsWrap').html(js2);
        } else if (rowCount == 9) {
            $('.statsWrap').html('');
            $('.statsWrap').html(js3);
        } else if (rowCount == 10) {
            $('.statsWrap').html('');
            $('.statsWrap').html(js2);
        } else if (rowCount == 11) {
            $('.statsWrap').html('');
            $('.statsWrap').html(js1);
        }
    });



    var options = {
        chart: {
            width: 480,
            height: 350,
            type: 'pie',
        },
        labels: ['LEADS', 'VISITS', 'CLOSED'],
        fill: {
            colors: ['#01b8aa', '#374649', '#fd625e']
        },
        colors: ['#01b8aa', '#374649', '#fd625e'],
        label: {
            position: 'bottom',
        },
        series: [250, 130, 24],
        responsive: [{
            breakpoint: 480,
            options: {
                chart: {
                    width: 200
                },
                legend: {
                    position: 'bottom'
                }
            }
        }]
    }

    var chart = new ApexCharts(
        document.querySelector("#chartpie"),
        options
    );

    chart.render();

    var options2 = {
        chart: {
            type: 'donut',
            width: 480,
            height: 350
        },
        labels: ['Qualify Lead', 'Develop', 'Propose', 'Close'],
        fill: {
            colors: ['#01b8aa', '#374649', '#f2c80f', '#fd625e']
        },
        colors: ['#01b8aa', '#374649', '#f2c80f', '#fd625e'],
        label: {
            position: 'bottom',
        },
        series: [250, 100, 30, 24],
        responsive: [{
            breakpoint: 480,
            options: {
                chart: {
                    width: 200
                },
                legend: {
                    position: 'bottom'
                }
            }
        }]
    }

    var chart2 = new ApexCharts(
         document.querySelector("#chartpie2"),
         options2
     );

    chart2.render();




    //var options = {
    //    chart: {
    //        height: 350,
    //        type: 'bar',
    //    },
    //    plotOptions: {
    //        bar: {
    //            horizontal: false,
    //        }
    //    },
    //    labels: ['Bankura', 'Bardwan', 'Darjeeling', 'Kolkata', 'Midnapore'],
    //    markers: {
    //        colors: ['#33b2df', '#546E7A', '#d4526e', '#13d8aa', '#A5978B', '#2b908f', '#f9a3a4', '#90ee7e', '#f48024']
    //    },
    //    dataLabels: {
    //        enabled: true
    //    },
    //    series: [{
    //        data: [2, 5, 9, 10, 12, 15, 20, 20, 35]
    //    }],
    //    xaxis: {
    //        categories: ['National Pension System', 'Regalia', 'Loan on Credit Card', 'Life Insurance', 'Education Loan', 'Personal Loan', 'Demat Account', '2 in 1 Account ', 'Two Wheeler Loans'],
    //    }
    //}

    //var chart = new ApexCharts(
    //     document.querySelector("#productPerformance112"),
    //     options
    // );

    //chart.render();


    $('.shLeader').click(function () {
        $('#shLeaderView').css({ 'visibility': 'visible' });
    });

    $('#tbsSales').click(function () {
        chart2.refresh();
    });

});

        $(document).ready(function () {
            $('body').on('click', '.panelAnchor', function () {
                $('#panelRight').toggleClass('open');
                //if( $('#panelRight').hasClass('open') ){
                //    $('body').append('<div class="overlay"></div>');
                //}else{
                //    $('.overlay').remove();
                //}
            });
            var options = {
                chart: {
                    height: 350,
                    type: 'line',
                },
                series: [{
                    name: 'Website Blog',
                    type: 'bar',
                    columnWidth: '50%',
                    data: [440, 505, 414, 671, 227, 413, 201, 352, 752, 320, 257, 160]
                },
                {
                    name: 'Website 2',
                    type: 'bar',
                    columnWidth: '50%',
                    data: [440, 505, 414, 671, 227, 413, 201, 352, 752, 320, 257, 160]
                },
                {
                    name: 'Social Media',
                    type: 'line',
                    data: [250, 242, 335, 27, 443, 422, 117, 331, 222, 422, 212, 116]
                }],
                stroke: {
                    width: [0, 0, 4]
                },
                title: {
                    text: 'Traffic Sources'
                },
                // labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
                labels: ['01 Jan 2001', '02 Jan 2001', '03 Jan 2001', '04 Jan 2001', '05 Jan 2001', '06 Jan 2001', '07 Jan 2001', '08 Jan 2001', '09 Jan 2001', '10 Jan 2001', '11 Jan 2001', '12 Jan 2001'],
                xaxis: {
                    type: 'datetime'
                },
                yaxis: [{
                    title: {
                        text: 'Website Blog',
                    },

                },
                //{
                //    opposite: true,
                //}
                ]

            }

            var chart = new ApexCharts(
              document.querySelector("#mixedChart"),
              options
            );

            chart.render();
        });


$(function () {
    $("#tbsalesman").click(function () {
        dbgridsalesman.Refresh();
        dbgridsalesman.Refresh();
    });
    $("#tbdistributor").click(function () {
        dbgriddistributor.Refresh();
        dbgriddistributor.Refresh();
    });
});

