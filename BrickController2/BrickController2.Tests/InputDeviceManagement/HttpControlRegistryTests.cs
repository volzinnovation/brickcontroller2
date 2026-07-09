using BrickController2.InputDeviceManagement.HttpControl;
using BrickController2.PlatformServices.InputDevice;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace BrickController2.Tests.InputDeviceManagement;

public class HttpControlRegistryTests
{
    [Fact]
    public void UpsertVirtualController_ReturnsWritableCapabilities()
    {
        var registry = new HttpControlRegistry();

        registry.UpsertVirtualController("Controller 1", "HTTP Controller 1", 1,
        [
            new CapabilityDefinitionRequest
            {
                Id = "leftStickX",
                EventType = InputDeviceEventType.Axis,
                EventCode = "LeftThumbStick_X",
                DisplayName = "Left stick X"
            }
        ]);

        var controller = registry.GetController("Controller 1");

        controller.Should().NotBeNull();
        controller!.Source.Should().Be(HttpControlControllerSource.Http);
        controller.Writable.Should().BeTrue();
        controller.Capabilities.Should().ContainSingle();
        controller.Capabilities[0].Should().BeEquivalentTo(new
        {
            Id = "leftStickX",
            EventType = InputDeviceEventType.Axis,
            EventCode = "LeftThumbStick_X",
            Writable = true,
            Value = 0f,
            MinValue = -1f,
            MaxValue = 1f
        });
    }

    [Fact]
    public void TrySetCapability_UpdatesValueAndReturnsInputEvent()
    {
        var registry = new HttpControlRegistry();
        registry.UpsertVirtualController("Controller 1", "HTTP Controller 1", 1,
        [
            new CapabilityDefinitionRequest
            {
                Id = "leftStickX",
                EventType = InputDeviceEventType.Axis,
                EventCode = "LeftThumbStick_X"
            }
        ]);

        var responses = registry.TrySetCapability(
            "Controller 1",
            new Dictionary<string, float> { ["leftStickX"] = 0.75f },
            out var events,
            out var error);

        error.Should().BeNull();
        responses.Should().ContainSingle();
        responses![0].Value.Should().Be(0.75f);
        events.Should().ContainSingle();
        events[(InputDeviceEventType.Axis, "LeftThumbStick_X")].Should().Be(0.75f);
        registry.GetCapability("Controller 1", "leftStickX")!.Value.Should().Be(0.75f);
    }

    [Fact]
    public void ObserveInputDeviceEvent_UpdatesVirtualCapabilityByEventCode()
    {
        var registry = new HttpControlRegistry();
        registry.UpsertVirtualController("Controller 1", "HTTP Controller 1", 1,
        [
            new CapabilityDefinitionRequest
            {
                Id = "leftStickX",
                EventType = InputDeviceEventType.Axis,
                EventCode = "LeftThumbStick_X"
            }
        ]);

        registry.ObserveInputDeviceEvent(new InputDeviceEventArgs(
            "Controller 1",
            InputDeviceEventType.Axis,
            "LeftThumbStick_X",
            0.5f));

        var controller = registry.GetController("Controller 1");

        controller!.Capabilities.Should().ContainSingle();
        controller.Capabilities[0].Id.Should().Be("leftStickX");
        controller.Capabilities[0].Value.Should().Be(0.5f);
        controller.Capabilities[0].Observed.Should().BeTrue();
    }

    [Fact]
    public void TrySetCapability_RejectsOutOfRangeValue()
    {
        var registry = new HttpControlRegistry();
        registry.UpsertVirtualController("Controller 1", "HTTP Controller 1", 1,
        [
            new CapabilityDefinitionRequest
            {
                Id = "buttonA",
                EventType = InputDeviceEventType.Button,
                EventCode = "Button_A"
            }
        ]);

        var responses = registry.TrySetCapability(
            "Controller 1",
            new Dictionary<string, float> { ["buttonA"] = 2f },
            out var events,
            out var error);

        responses.Should().BeNull();
        events.Should().BeEmpty();
        error.Should().Contain("outside");
    }

    [Fact]
    public void TrySetCapability_RejectsRemovedVirtualControllerWithoutMutatingValue()
    {
        var registry = new HttpControlRegistry();
        registry.UpsertVirtualController("Controller 1", "HTTP Controller 1", 1,
        [
            new CapabilityDefinitionRequest
            {
                Id = "leftStickX",
                EventType = InputDeviceEventType.Axis,
                EventCode = "LeftThumbStick_X"
            }
        ]);

        registry.RemoveVirtualController("Controller 1");

        var responses = registry.TrySetCapability(
            "Controller 1",
            new Dictionary<string, float> { ["leftStickX"] = 0.75f },
            out var events,
            out var error);

        responses.Should().BeNull();
        events.Should().BeEmpty();
        error.Should().Contain("read-only");

        var controller = registry.GetController("Controller 1");
        controller.Should().NotBeNull();
        controller!.Status.Should().Be(HttpControlControllerStatus.Disconnected);
        controller.Writable.Should().BeFalse();
        controller.Capabilities[0].Writable.Should().BeFalse();
        controller.Capabilities[0].Value.Should().Be(0f);
    }

    [Fact]
    public void ObserveInputDeviceEvent_CreatesObservedReadOnlyCapability()
    {
        var registry = new HttpControlRegistry();

        registry.ObserveInputDeviceEvent(new InputDeviceEventArgs(
            "Controller 2",
            InputDeviceEventType.Button,
            "Button_A",
            1f));

        var controller = registry.GetController("Controller 2");

        controller.Should().NotBeNull();
        controller!.Writable.Should().BeFalse();
        controller.Capabilities.Should().ContainSingle();
        controller.Capabilities[0].Should().BeEquivalentTo(new
        {
            Id = "Button_A",
            EventType = InputDeviceEventType.Button,
            EventCode = "Button_A",
            Writable = false,
            Observed = true,
            Value = 1f
        });
    }
}
