x-OSC-Latency-Tester
====================

This repository includes the source files for the tools used to evaluated the closed-loop latency of [x-OSC](http://www.x-io.co.uk/smc-2013/www.x-io.co.uk/x-osc) communicating with a host.  The closed-loop latency is the delay between a physical change on an input pin and the resulting physical change on an output pin.  This delay includes: sampling of x-OSC input pin, WiFi transmission from x-OSC to computer, processing of received data by computer, WiFi transmission from computer to x-OSC, setting x-OSC output pin.

A windows application and simple hardware setup allow the latency to be measured repeatedly and autonomously.  In this setup, an x-OSC digital input is connected to a 1 Hz square wave while the Windows application communicating with x-OSC creates the behaviour: *output = input*.  Both the input and output are connected to an [XOR gate](http://en.wikipedia.org/wiki/XOR_gate) to generate a pulse width equal to the delay between the input and output pin state changes.  This pulse width is measured using a [TTi TF930 frequency counter](http://www.tti-test.com/products-tti/rf/frequency-counters.htm) which sends the measurement to the Windows application via USB to be logged to a [CSV file](http://en.wikipedia.org/wiki/Comma-separated_values).

The repository includes the results of investigations into the closed-lop latency for various scenarios and a MATLAB script for plotting these distributions.  Images of the hardware setup and ultimate distribution plots have also been included.

The method for evaluating latency was original presented in a [2013 paper]([Paper presented at SMC 2013](http://www.x-io.co.uk/smc-2013/) at the Sound and Music Computing conference.

Hardware Setup
--------------

<div align="center">
<img src="https://raw.github.com/xioTechnologies/x-OSC-Latency-Tester/master/Hardware%20Setup.png"/>
</div>

Ad hoc latency distribution
---------------------------

<div align="center">
<img src="https://raw.github.com/xioTechnologies/x-OSC-Latency-Tester/master/Ad%20Hoc%20Distribution.png"/>
</div>
